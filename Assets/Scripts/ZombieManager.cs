using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Tüm zombileri takip eder ve hepsi öldüğünde kazanma ekranını gösterir
/// </summary>
public class ZombieManager : MonoBehaviour
{
    [Header("Victory Settings")]
    [Tooltip("Tüm zombiler öldüğünde kazanma ekranını göstermek için bekleme süresi (saniye)")]
    public float delayBeforeVictoryScreen = 1.5f;
    
    [Tooltip("Kazanma ekranı sahnesinin adı")]
    public string victorySceneName = "WinScene";
    
    [Tooltip("Eğer sahne yerine in-game UI kullanmak istersen, bu GameObject'i ayarla")]
    public GameObject victoryUIPanel;

    private List<HealthSystem> allZombies = new List<HealthSystem>();
    private bool victoryTriggered = false;

    public static ZombieManager Instance { get; private set; }

    void Awake()
    {
        // Singleton pattern - sadece bir tane ZombieManager olmalı
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("Birden fazla ZombieManager bulundu! Sadece bir tane olmalı.");
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        // Sahnedeki tüm zombileri bul ve listeye ekle
        FindAllZombies();
        
        Debug.Log("🧟 ZombieManager başlatıldı. Toplam " + allZombies.Count + " zombi bulundu.");
        
        // Eğer hiç zombi bulunamadıysa uyarı ver ve gecikmeli kontrol başlat
        if (allZombies.Count == 0)
        {
            Debug.LogWarning("⚠️ ZombieManager: Sahne başlangıcında hiç zombi bulunamadı! Zombiler runtime'da spawn ediliyor olabilir veya HealthSystem henüz eklenmemiş olabilir. Gecikmeli kontrol başlatılıyor...");
            StartCoroutine(DelayedZombieCheck());
        }
    }
    
    System.Collections.IEnumerator DelayedZombieCheck()
    {
        yield return new WaitForSeconds(0.5f);
        FindAllZombies();
        Debug.Log("🧟 ZombieManager gecikmeli kontrol: Toplam " + allZombies.Count + " zombi bulundu.");
    }

    /// <summary>
    /// Sahnedeki tüm zombileri bulur ve listeye ekler
    /// </summary>
    void FindAllZombies()
    {
        allZombies.Clear();
        
        // HealthSystem component'ine sahip tüm objeleri bul
        HealthSystem[] allHealthSystems;
        #if UNITY_2023_1_OR_NEWER
        allHealthSystems = FindObjectsByType<HealthSystem>(FindObjectsSortMode.None);
        #else
        allHealthSystems = FindObjectsOfType<HealthSystem>();
        #endif
        
        foreach (HealthSystem healthSystem in allHealthSystems)
        {
            // Player'ı hariç tut (sadece zombileri say)
            if (healthSystem != null && !healthSystem.isPlayerObject)
            {
                allZombies.Add(healthSystem);
            }
        }
    }

    /// <summary>
    /// Zombi öldüğünde bu fonksiyon çağrılır
    /// </summary>
    public void OnZombieDied(HealthSystem zombieHealth)
    {
        if (victoryTriggered)
        {
            Debug.Log("ℹ️ Kazanma ekranı zaten tetiklendi, yeni ölüm bildirimi yok sayılıyor.");
            return; // Zaten kazanma ekranı gösterildiyse tekrar kontrol etme
        }
        
        if (zombieHealth == null)
        {
            Debug.LogWarning("⚠️ ZombieManager: Null zombi ölüm bildirimi alındı!");
            return;
        }
        
        Debug.Log("💀 Zombi öldü: " + zombieHealth.gameObject.name + ". Kalan zombiler kontrol ediliyor...");
        
        // Ölü zombileri listeden çıkar
        if (allZombies.Contains(zombieHealth))
        {
            allZombies.Remove(zombieHealth);
            Debug.Log("📋 Zombi listeden çıkarıldı. Kalan zombi sayısı: " + allZombies.Count);
        }
        else
        {
            Debug.LogWarning("⚠️ Ölen zombi listede bulunamadı: " + zombieHealth.gameObject.name);
            Debug.LogWarning("⚠️ Bu zombi muhtemelen başlangıçta listeye eklenmemiş. Şimdi ekleniyor ve listeden çıkarılıyor...");
        }

        // Tüm zombiler öldü mü kontrol et
        CheckForVictory();
    }

    /// <summary>
    /// Tüm zombilerin ölüp ölmediğini kontrol eder
    /// </summary>
    void CheckForVictory()
    {
        // Önce null referansları temizle (zombi objesi destroy edilmiş olabilir)
        int removedCount = allZombies.RemoveAll(z => z == null);
        if (removedCount > 0)
        {
            Debug.Log("🧹 " + removedCount + " null zombi referansı temizlendi.");
        }

        // Canlı zombi sayısını kontrol et
        int aliveZombies = 0;
        foreach (HealthSystem zombie in allZombies)
        {
            if (zombie != null && !zombie.IsDead)
            {
                aliveZombies++;
            }
        }

        Debug.Log("🔍 Zombi kontrolü: Toplam listede " + allZombies.Count + ", Canlı: " + aliveZombies);

        // Eğer listede zombi yoksa veya hepsi öldüyse kazanma ekranını göster
        if (aliveZombies == 0)
        {
            Debug.Log("✅ Tüm zombiler öldü! Kazanma ekranı tetikleniyor...");
            TriggerVictory();
        }
    }

    /// <summary>
    /// Kazanma ekranını gösterir
    /// </summary>
    void TriggerVictory()
    {
        if (victoryTriggered)
        {
            Debug.LogWarning("⚠️ TriggerVictory() zaten çağrılmış, tekrar çağrılmıyor.");
            return;
        }
        victoryTriggered = true;

        Debug.Log("🎉 TÜM ZOMBİLER ÖLDÜ! KAZANDINIZ!");
        Debug.Log("⏱️ " + delayBeforeVictoryScreen + " saniye sonra WinScene yüklenecek...");

        // Belirlenen süre kadar bekle, sonra kazanma ekranını göster
        Invoke("ShowVictoryScreen", delayBeforeVictoryScreen);
    }

    /// <summary>
    /// Kazanma ekranını gösterir (sahne veya UI panel)
    /// </summary>
    void ShowVictoryScreen()
    {
        Debug.Log("🎬 ShowVictoryScreen() çağrıldı!");
        Debug.Log("🔍 Victory Scene Name: '" + victorySceneName + "'");
        Debug.Log("🔍 Victory UI Panel: " + (victoryUIPanel != null ? victoryUIPanel.name : "null"));
        
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
            Debug.Log("🔍 Sahne adı: '" + victorySceneName + "'");
            
            // Önce build index ile dene (daha güvenilir)
            int sceneIndex = -1;
            for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
            {
                string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
                string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);
                if (sceneName == victorySceneName)
                {
                    sceneIndex = i;
                    Debug.Log("✅ Sahne bulundu! Build Index: " + i + ", Path: " + scenePath);
                    break;
                }
            }
            
            if (sceneIndex >= 0)
            {
                // Build index ile yükle (daha güvenilir)
                Debug.Log("📦 Build index ile yükleniyor: " + sceneIndex);
                SceneManager.LoadScene(sceneIndex);
            }
            else
            {
                // Build index bulunamadı, sahne adı ile dene
                Debug.LogWarning("⚠️ Build index bulunamadı, sahne adı ile deneniyor: " + victorySceneName);
                try
                {
                    SceneManager.LoadScene(victorySceneName);
                    Debug.Log("✅ Sahne adı ile yükleme komutu verildi: " + victorySceneName);
                }
                catch (System.Exception e)
                {
                    Debug.LogError("❌ HATA: '" + victorySceneName + "' sahnesi yüklenemedi!");
                    Debug.LogError("Hata detayı: " + e.Message);
                    Debug.LogError("💡 Lütfen şunları kontrol edin:");
                    Debug.LogError("   1. WinScene sahnesi Build Settings'e ekli mi?");
                    Debug.LogError("   2. Sahne adı tam olarak 'WinScene' mi? (büyük/küçük harf önemli)");
                }
            }
        }
        else
        {
            Debug.LogWarning("⚠️ Kazanma ekranı için ne sahne ne de UI paneli ayarlanmış! Lütfen ZombieManager'da ayarlayın.");
        }
    }

    /// <summary>
    /// Runtime'da yeni zombi eklendiğinde çağrılabilir
    /// </summary>
    public void RegisterZombie(HealthSystem zombieHealth)
    {
        if (zombieHealth == null)
        {
            Debug.LogWarning("⚠️ ZombieManager: Null zombi kayıt edilmeye çalışıldı!");
            return;
        }
        
        if (zombieHealth.isPlayerObject)
        {
            Debug.LogWarning("⚠️ ZombieManager: Player objesi zombi olarak kaydedilmeye çalışıldı: " + zombieHealth.gameObject.name);
            return;
        }
        
        if (!allZombies.Contains(zombieHealth))
        {
            allZombies.Add(zombieHealth);
            Debug.Log("🧟 Yeni zombi eklendi: " + zombieHealth.gameObject.name + ". Toplam: " + allZombies.Count);
        }
        else
        {
            Debug.Log("ℹ️ Zombi zaten listede: " + zombieHealth.gameObject.name);
        }
    }

    /// <summary>
    /// Debug için: Canlı zombi sayısını döndürür
    /// </summary>
    public int GetAliveZombieCount()
    {
        int count = 0;
        foreach (HealthSystem zombie in allZombies)
        {
            if (zombie != null && !zombie.IsDead)
            {
                count++;
            }
        }
        return count;
    }
}

