using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public float damage = 25f;
    public float attackRange = 2.3f;
    public LayerMask enemyLayer;

    public Transform attackPoint;
    public Animator animator;

    public float hitDelay = 0.4f; // animasyona göre ayarla

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Attack();
        }
    }

    void Attack()
    {
        if (animator != null)
            animator.SetTrigger("Attack");

        Collider[] hits = Physics.OverlapSphere(
            attackPoint.position,
            attackRange,
            enemyLayer
        );

        foreach (Collider hit in hits)
        {
            // Only Zombies
            if (!hit.CompareTag("Zombie"))
                continue;

            HealthSystem health = hit.GetComponentInParent<HealthSystem>();
            if (health != null)
            {
                health.TakeDamage(damage);
            }
        }
    }


    void DoDamage()
    {
        Collider[] hits = Physics.OverlapSphere(
            attackPoint.position,
            attackRange,
            enemyLayer
        );

        foreach (Collider hit in hits)
        {
            HealthSystem hs = hit.GetComponent<HealthSystem>();
            if (hs != null)
            {
                Debug.Log("HIT: " + hit.name);
                hs.TakeDamage(damage);
            }
        }
    }
}