using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement; // Sahne geçişi için şart

public class HealthSystem : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    public float currentHealth = 100f;

    [Header("Configuration")]
    [Tooltip("Bu script Player üzerindeyse bunu MUTLAKA işaretle!")]
    public bool isPlayerObject = false;
    public bool showHealthBar = true;

    [Header("Death Settings")]
    public float delayBeforeLoad = 2.5f; // Ölüm animasyonunu izleme süresi

    [Header("Zombie Death Freeze")]
    [Tooltip("Zombi ölünce AI scriptleri kapansın mı?")]
    public bool disableAllBehaviourScriptsOnDeath = true;

    [Tooltip("Zombi ölünce colliderlar kapansın mı?")]
    public bool disableCollidersOnDeath = true;

    [Tooltip("Zombi ölünce ölüm animasyonunun sonunda Animator donsun mu? (Son karede kalır)")]
    public bool freezeAnimatorOnDeath = true;

    [Tooltip("Animator'ın Die state adı. Animator'daki state adı ile aynı olmalı.")]
    public string dieStateName = "Die";

    // Durum değişkenleri
    private bool isDead = false;
    private bool keepPositionFixed = false; // Ceset kaymasın diye pozisyonu kilitler
    private Vector3 deathPosition; // Ölüm anındaki koordinat

    // Bileşenler
    private Animator animator;
    private CharacterController characterController;
    private MonoBehaviour[] cachedScripts;
    private Collider[] cachedColliders;
    private Rigidbody rb;

    public bool IsDead => isDead;

    void Start()
    {
        currentHealth = maxHealth;

        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();

        // Zombi için: tüm script/collider'ları önceden cache'le (ölümde hızlı kapatmak için)
        cachedScripts = GetComponents<MonoBehaviour>();
        cachedColliders = GetComponentsInChildren<Collider>(true);

        // Debug: Health bar ayarlarını kontrol et
        if (!isPlayerObject)
        {
            Debug.Log("🧟 " + gameObject.name + " - HealthSystem başlatıldı. showHealthBar: " + showHealthBar + ", MaxHealth: " + maxHealth);
            
            // ZombieManager'a kaydet (eğer varsa ve henüz kayıtlı değilse)
            if (ZombieManager.Instance != null)
            {
                ZombieManager.Instance.RegisterZombie(this);
            }
        }
    }

    void Update()
    {
        // Öldükten sonra sahne yüklenene kadar pozisyonu çivile (keepPositionFixed)
        if (keepPositionFixed && isDead)
        {
            transform.position = deathPosition;
        }
    }

    public void TakeDamage(float damage)
    {
        // Zaten ölüyse hasar verme
        if (currentHealth <= 0 || isDead) return;

        // Canı azalt (gerçek işlem)
        currentHealth -= damage;
        currentHealth = Mathf.Max(0, currentHealth); // Negatif olmasın

        // Can 0 veya altındaysa öldür (gerçek işlem)
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        if (isDead) return;
        currentHealth += amount;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
    }

    // HealthSystem.cs  (ONLY the Die() method - replace yours with this)

    void Die()
    {
        if (isDead) return;
        isDead = true;

        if (isPlayerObject)
        {
            HandlePlayerDeath();
            return;
        }

        // Lock corpse
        deathPosition = transform.position;
        keepPositionFixed = true;

        // HARD STOP: stop ALL attack components in children (in case they live on a child object)
        var attacks = GetComponentsInChildren<ZombieMeleeAttackTimed>(true);
        for (int i = 0; i < attacks.Length; i++)
        {
            if (attacks[i] != null) attacks[i].enabled = false; // triggers OnDisable() and stops coroutine
        }

        // Stop any Invoke() loops on this zombie and its children (e.g., ZombieScreamTimer)
        CancelInvoke();
        var childBehaviours = GetComponentsInChildren<MonoBehaviour>(true);
        for (int i = 0; i < childBehaviours.Length; i++)
        {
            if (childBehaviours[i] != null) childBehaviours[i].CancelInvoke();
        }

        // Stop movement/physics
        if (characterController != null) characterController.enabled = false;

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
        }

        // Disable other scripts so they cannot change animator params
        if (disableAllBehaviourScriptsOnDeath)
        {
            var scripts = GetComponentsInChildren<MonoBehaviour>(true); // IMPORTANT: includes children now
            for (int i = 0; i < scripts.Length; i++)
            {
                MonoBehaviour s = scripts[i];
                if (s == null) continue;
                if (s == this) continue; // HealthSystem stays enabled
                s.enabled = false;
            }
        }

        // Disable colliders (best practice: disable triggers only so corpse can still block the player)
        if (disableCollidersOnDeath)
        {
            var cols = GetComponentsInChildren<Collider>(true);
            for (int i = 0; i < cols.Length; i++)
            {
                Collider c = cols[i];
                if (c == null) continue;

                if (c.isTrigger) c.enabled = false; // <-- key to stop damage triggers forever
            }
        }

        if (animator == null)
        {
            Debug.LogError("🧟 " + gameObject.name + " - Animator yok, ölüm animasyonu oynatılamıyor.");
            return;
        }

        animator.applyRootMotion = false;

        // Reset other triggers
        animator.ResetTrigger("Attack");
        animator.ResetTrigger("Scream");

        // Try trigger if it exists
        if (HasTrigger(animator, "Die"))
            animator.SetTrigger("Die");

        // Also set IsDead bool if it exists
        if (HasBool(animator, "IsDead"))
            animator.SetBool("IsDead", true);

        // Force-play Die state directly
        animator.Play(dieStateName, 0, 0f);

        // Zombi öldüğünde ZombieManager'a bildir
        if (ZombieManager.Instance != null)
        {
            ZombieManager.Instance.OnZombieDied(this);
        }

        // Freeze on last frame
        if (freezeAnimatorOnDeath)
            StartCoroutine(FreezeAnimatorAfterDeath());
    }


    private bool HasTrigger(Animator a, string name)
    {
        foreach (var p in a.parameters)
            if (p.name == name && p.type == AnimatorControllerParameterType.Trigger)
                return true;
        return false;
    }

    private bool HasBool(Animator a, string name)
    {
        foreach (var p in a.parameters)
            if (p.name == name && p.type == AnimatorControllerParameterType.Bool)
                return true;
        return false;
    }


    private IEnumerator FreezeAnimatorAfterDeath()
    {
        // Die state'e gerçekten girene kadar kısa süre bekle (blend/geçişler için)
        yield return null;
        yield return new WaitForSeconds(0.05f);

        if (animator == null) yield break;

        // Die state'e girene kadar bekle (max bekleme güvenliği ile)
        float safety = 2f;
        float t = 0f;

        while (t < safety)
        {
            AnimatorStateInfo st = animator.GetCurrentAnimatorStateInfo(0);
            if (st.IsName(dieStateName))
                break;

            t += Time.deltaTime;
            yield return null;
        }

        // Die state süresini bekle, sonra dondur
        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);

        // Eğer Die'a giremediyse yine de küçük bir süre bekleyip dondur
        float wait = state.length > 0.01f ? state.length : 0.8f;

        yield return new WaitForSeconds(wait);

        // Son pozda kalması için animator'ı dondur
        animator.speed = 0f;

        // Alternatif: animator.enabled = false; (pose kalır ama bazı projelerde farklı davranabilir)
        // animator.enabled = false;
    }

    void HandlePlayerDeath()
    {
        Debug.Log("💀 OYUNCU ÖLDÜ! Pozisyon kilitlendi, animasyon oynatılıyor...");

        // 1. Ölüm anındaki pozisyonu kaydet
        deathPosition = transform.position;
        keepPositionFixed = true; // Update fonksiyonunda kitlemeyi başlat

        // 2. Fizik ve hareketi kapat (Düşmeyi/Kaymayı önler)
        if (characterController != null) characterController.enabled = false;

        // Varsa Rigidbody'yi de dondur
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
        }

        // 3. Ölüm animasyonunu oynat
        if (animator != null)
        {
            animator.applyRootMotion = false; // Animasyon karakteri hareket ettirmesin
            animator.SetTrigger("Die");
        }

        // 4. Belirlenen süre kadar bekle, sonra sahneyi değiştir
        Invoke(nameof(LoadDeadScreenScene), delayBeforeLoad);
    }

    void LoadDeadScreenScene()
    {
        Debug.Log("🎬 DeadScreen sahnesine geçiliyor...");

        // Mouse imlecini serbest bırak (Menüde tıklayabilmek için)
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Zamanı normale döndür
        Time.timeScale = 1f;

        // Sahneyi yükle
        SceneManager.LoadScene("DeadScreen");
    }

    // Basit GUI Can Barı
    void OnGUI()
    {
        if (!showHealthBar || isDead) return;

        float healthPercent = currentHealth / maxHealth;

        // Player için ekranın sol üst köşesinde health bar göster
        if (isPlayerObject)
        {
            float barWidth = 150f;
            float barHeight = 15f;
            float offsetX = 15f;
            float offsetY = 15f;

            // Arka plan (kırmızı)
            GUI.color = Color.red;
            GUI.DrawTexture(new Rect(offsetX, offsetY, barWidth, barHeight), Texture2D.whiteTexture);

            // Can barı (yeşil -> kırmızı)
            GUI.color = Color.Lerp(Color.red, Color.green, healthPercent);
            GUI.DrawTexture(new Rect(offsetX, offsetY, barWidth * healthPercent, barHeight), Texture2D.whiteTexture);

            // Kenarlık
            GUI.color = Color.white;
            DrawRectOutline(new Rect(offsetX, offsetY, barWidth, barHeight), 1f);

            // Can yüzdesi yazısı (daha küçük font)
            GUI.color = Color.white;
            GUIStyle labelStyle = new GUIStyle();
            labelStyle.alignment = TextAnchor.MiddleCenter;
            labelStyle.normal.textColor = Color.white;
            labelStyle.fontSize = 11;
            GUI.Label(new Rect(offsetX, offsetY, barWidth, barHeight),
                     "HP: " + Mathf.Ceil(currentHealth) + "/" + maxHealth,
                     labelStyle);

            return;
        }

        // Zombiler için kafalarının üstünde health bar göster
        Camera mainCam = Camera.main;
        if (mainCam == null)
        {
            // Eğer Camera.main yoksa, sahnedeki tüm kameraları kontrol et
            Camera[] cameras;
            #if UNITY_2023_1_OR_NEWER
            cameras = FindObjectsByType<Camera>(FindObjectsSortMode.None);
            #else
            cameras = FindObjectsOfType<Camera>();
            #endif
            if (cameras.Length > 0)
            {
                mainCam = cameras[0];
            }
            else
            {
                return;
            }
        }

        // Zombinin başının üstünde bir nokta hesapla (yükseklik ayarlanabilir)
        Vector3 worldPos = transform.position + Vector3.up * 2.2f;
        Vector3 screenPos = mainCam.WorldToScreenPoint(worldPos);

        // Eğer zombi kameranın arkasındaysa gösterme
        if (screenPos.z > 0)
        {
            float barWidth = 60f;
            float barHeight = 6f;
            float offsetY = 25f; // Bar'ın zombinin üstünde ne kadar yukarıda olacağı

            // Arka plan (koyu kırmızı/siyah) - her zaman göster
            GUI.color = new Color(0.2f, 0f, 0f, 0.8f);
            GUI.DrawTexture(new Rect(screenPos.x - barWidth / 2, Screen.height - screenPos.y - offsetY, barWidth, barHeight), Texture2D.whiteTexture);

            // Can barı (yeşil -> kırmızı) - healthPercent'e göre
            if (healthPercent > 0)
            {
                GUI.color = Color.Lerp(Color.red, Color.green, healthPercent);
                GUI.DrawTexture(new Rect(screenPos.x - barWidth / 2, Screen.height - screenPos.y - offsetY, barWidth * healthPercent, barHeight), Texture2D.whiteTexture);
            }

            // Kenarlık (beyaz, ince)
            GUI.color = Color.white;
            DrawRectOutline(new Rect(screenPos.x - barWidth / 2, Screen.height - screenPos.y - offsetY, barWidth, barHeight), 0.5f);
        }
    }

    // Kenarlık çizmek için yardımcı fonksiyon
    void DrawRectOutline(Rect rect, float thickness)
    {
        // Üst
        GUI.DrawTexture(new Rect(rect.x, rect.y, rect.width, thickness), Texture2D.whiteTexture);
        // Alt
        GUI.DrawTexture(new Rect(rect.x, rect.y + rect.height - thickness, rect.width, thickness), Texture2D.whiteTexture);
        // Sol
        GUI.DrawTexture(new Rect(rect.x, rect.y, thickness, rect.height), Texture2D.whiteTexture);
        // Sağ
        GUI.DrawTexture(new Rect(rect.x + rect.width - thickness, rect.y, thickness, rect.height), Texture2D.whiteTexture);
    }
}