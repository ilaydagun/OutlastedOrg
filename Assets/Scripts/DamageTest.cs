using UnityEngine;

public class DamageTest : MonoBehaviour
{
    private HealthSystem health;

    void Start()
    {
        health = GetComponent<HealthSystem>();
        Debug.Log("🔧 Manuel hasar test sistemi hazır! T tuşuna basarak 15 hasar ver.");
    }

    void Update()
    {
        // T tuşuna basınca hasar ver
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (health != null)
            {
                health.TakeDamage(15);
                Debug.Log("✅ Manuel hasar verildi!");
            }
        }
    }
}