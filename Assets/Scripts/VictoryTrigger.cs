using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Oyuncu bu trigger zone'a girdiğinde kazanma ekranını gösterir
/// </summary>
[RequireComponent(typeof(Collider))]
public class VictoryTrigger : MonoBehaviour
{
    [Header("Victory Settings")]
    [Tooltip("Kazanma ekranı sahnesinin adı")]
    public string victorySceneName = "VictoryScreen";
    
    [Tooltip("Eğer sahne yerine in-game UI kullanmak istersen, bu GameObject'i ayarla")]
    public GameObject victoryUIPanel;
    
    [Tooltip("Kazanma ekranını göstermeden önce bekleme süresi (saniye)")]
    public float delayBeforeVictory = 0.5f;
    
    [Header("Visual Settings")]
    [Tooltip("Kırmızı çizgi görseli (Line Renderer veya GameObject)")]
    public GameObject redLineVisual;
    
    [Tooltip("Trigger'ın etiketlenmesi gereken tag (Player)")]
    public string targetTag = "Player";
    
    private bool victoryTriggered = false;

    void Start()
    {
        // Collider'ın trigger olması gerekiyor
        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.isTrigger = true;
        }
        else
        {
            Debug.LogError("❌ VictoryTrigger: Collider bulunamadı! Lütfen bir Collider ekleyin.");
        }
        
        // Kırmızı çizgi görselini başlangıçta göster
        if (redLineVisual != null)
        {
            redLineVisual.SetActive(true);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Sadece Player tag'ine sahip objeler için çalış
        if (victoryTriggered) return;
        
        if (other.CompareTag(targetTag))
        {
            Debug.Log("🎉 Oyuncu kazanma bölgesine girdi! Kazanma ekranı gösteriliyor...");
            victoryTriggered = true;
            
            // Belirlenen süre kadar bekle, sonra kazanma ekranını göster
            Invoke("ShowVictoryScreen", delayBeforeVictory);
        }
    }

    void ShowVictoryScreen()
    {
        // Mouse imlecini serbest bırak
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Zamanı normale döndür
        Time.timeScale = 1f;

        // Eğer UI panel varsa onu göster, yoksa sahneyi yükle
        if (victoryUIPanel != null)
        {
            victoryUIPanel.SetActive(true);
            Debug.Log("🎬 Kazanma UI paneli gösteriliyor...");
        }
        else if (!string.IsNullOrEmpty(victorySceneName))
        {
            Debug.Log("🎬 " + victorySceneName + " sahnesine geçiliyor...");
            
            // Sahneyi yükle
            SceneManager.LoadScene(victorySceneName);
        }
        else
        {
            Debug.LogWarning("⚠️ Kazanma ekranı için ne sahne ne de UI paneli ayarlanmış! Lütfen VictoryTrigger'da ayarlayın.");
        }
    }

    // Gizmo çizimi (Scene view'da görünür)
    void OnDrawGizmos()
    {
        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            Gizmos.color = new Color(1f, 0f, 0f, 0.3f); // Kırmızı, yarı saydam
            Gizmos.matrix = transform.localToWorldMatrix;
            
            if (col is BoxCollider)
            {
                BoxCollider box = col as BoxCollider;
                Gizmos.DrawCube(box.center, box.size);
            }
            else if (col is SphereCollider)
            {
                SphereCollider sphere = col as SphereCollider;
                Gizmos.DrawSphere(sphere.center, sphere.radius);
            }
            else if (col is CapsuleCollider)
            {
                CapsuleCollider capsule = col as CapsuleCollider;
                // Capsule için basit bir kutu çiz
                Gizmos.DrawCube(capsule.center, new Vector3(capsule.radius * 2, capsule.height, capsule.radius * 2));
            }
            
            // Kırmızı kenarlık çiz
            Gizmos.color = Color.red;
            if (col is BoxCollider)
            {
                BoxCollider box = col as BoxCollider;
                DrawWireCube(box.center, box.size);
            }
        }
    }

    void DrawWireCube(Vector3 center, Vector3 size)
    {
        Vector3 halfSize = size * 0.5f;
        Vector3[] corners = new Vector3[]
        {
            center + new Vector3(-halfSize.x, -halfSize.y, -halfSize.z),
            center + new Vector3(halfSize.x, -halfSize.y, -halfSize.z),
            center + new Vector3(halfSize.x, halfSize.y, -halfSize.z),
            center + new Vector3(-halfSize.x, halfSize.y, -halfSize.z),
            center + new Vector3(-halfSize.x, -halfSize.y, halfSize.z),
            center + new Vector3(halfSize.x, -halfSize.y, halfSize.z),
            center + new Vector3(halfSize.x, halfSize.y, halfSize.z),
            center + new Vector3(-halfSize.x, halfSize.y, halfSize.z)
        };

        // Alt yüz
        DrawLine(corners[0], corners[1]);
        DrawLine(corners[1], corners[2]);
        DrawLine(corners[2], corners[3]);
        DrawLine(corners[3], corners[0]);
        
        // Üst yüz
        DrawLine(corners[4], corners[5]);
        DrawLine(corners[5], corners[6]);
        DrawLine(corners[6], corners[7]);
        DrawLine(corners[7], corners[4]);
        
        // Dikey kenarlar
        DrawLine(corners[0], corners[4]);
        DrawLine(corners[1], corners[5]);
        DrawLine(corners[2], corners[6]);
        DrawLine(corners[3], corners[7]);
    }

    void DrawLine(Vector3 start, Vector3 end)
    {
        Gizmos.DrawLine(transform.TransformPoint(start), transform.TransformPoint(end));
    }
}

