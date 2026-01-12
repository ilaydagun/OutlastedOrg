using UnityEngine;

/// <summary>
/// Basitleştirilmiş Damage Dealer - Debug için
/// </summary>
public class SimpleDamageDebug : MonoBehaviour
{
    public float damageAmount = 15f;
    public string targetTag = "Player";

    void Start()
    {
        Debug.Log($"✅ {gameObject.name} DamageDealer hazır! Hedef: {targetTag}");
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"🔴 Çarpışma! {gameObject.name} ↔ {collision.gameObject.name}");

        if (collision.gameObject.CompareTag(targetTag))
        {
            // Eski HealthSystem'i dene
            HealthSystem health1 = collision.gameObject.GetComponent<HealthSystem>();
            if (health1 != null)
            {
                health1.TakeDamage(damageAmount);
                Debug.Log($"⚔️ HealthSystem ile hasar verildi!");
                return;
            }

            // SimpleHealthDebug'ı dene
            HealthSystem health2 = collision.gameObject.GetComponent<HealthSystem>();
            if (health2 != null)
            {
                health2.TakeDamage(damageAmount);
                Debug.Log($"⚔️ SimpleHealthDebug ile hasar verildi!");
                return;
            }

            Debug.LogWarning($"⚠️ {collision.gameObject.name} üzerinde health component bulunamadı!");
        }
        else
        {
            Debug.Log($"⚠️ Tag uyuşmuyor. Hedef: {targetTag}, Çarpan: {collision.gameObject.tag}");
        }
    }
}