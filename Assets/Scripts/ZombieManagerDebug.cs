using UnityEngine;

/// <summary>
/// ZombieManager durumunu ekranda gösterir (debug için)
/// </summary>
public class ZombieManagerDebug : MonoBehaviour
{
    [Header("Debug Settings")]
    [Tooltip("Debug bilgilerini ekranda göster")]
    public bool showDebugInfo = true;
    
    [Tooltip("Debug bilgisinin ekrandaki konumu")]
    public Vector2 debugPosition = new Vector2(10, 100);

    void OnGUI()
    {
        if (!showDebugInfo) return;
        
        if (ZombieManager.Instance == null)
        {
            GUI.color = Color.red;
            GUI.Label(new Rect(debugPosition.x, debugPosition.y, 400, 30), 
                     "❌ ZombieManager bulunamadı! Sahneye ekleyin.");
            return;
        }

        GUI.color = Color.white;
        int aliveCount = ZombieManager.Instance.GetAliveZombieCount();
        
        string sceneName = string.IsNullOrEmpty(ZombieManager.Instance.victorySceneName) 
            ? "Ayarlanmamış" 
            : ZombieManager.Instance.victorySceneName;
        
        string debugText = "🧟 ZombieManager Durumu:\n" +
                          "Canlı Zombiler: " + aliveCount + "\n" +
                          "Victory Scene: " + sceneName;
        
        GUI.Label(new Rect(debugPosition.x, debugPosition.y, 400, 100), debugText);
    }
}

