using UnityEngine;

/// <summary>
/// Zombilere otomatik olarak HealthSystem ekler ve ayarlarını yapar
/// </summary>
[RequireComponent(typeof(ZombieChase))]
public class ZombieHealthSetup : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    public bool showHealthBar = true;

    void Start()
    {
        // HealthSystem var mı kontrol et
        HealthSystem healthSystem = GetComponent<HealthSystem>();
        
        if (healthSystem == null)
        {
            // Yoksa ekle
            healthSystem = gameObject.AddComponent<HealthSystem>();
            Debug.Log("✅ " + gameObject.name + " üzerine HealthSystem eklendi!");
        }
        
        // Ayarları yap
        healthSystem.maxHealth = maxHealth;
        healthSystem.currentHealth = maxHealth;
        healthSystem.isPlayerObject = false;
        healthSystem.showHealthBar = showHealthBar;
        
        // ZombieManager'a kaydet (eğer varsa)
        if (ZombieManager.Instance != null)
        {
            ZombieManager.Instance.RegisterZombie(healthSystem);
        }
        
        Debug.Log("🧟 " + gameObject.name + " - HealthSystem hazır! MaxHealth: " + maxHealth + ", ShowHealthBar: " + showHealthBar);
    }
}

