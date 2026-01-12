using UnityEngine;

public class ZombieScreamTimer : MonoBehaviour
{
    private Animator animator;
    public float screamInterval = 10f;

    private HealthSystem healthSystem;

    void Start()
    {
        animator = GetComponent<Animator>();
        healthSystem = GetComponent<HealthSystem>();

        InvokeRepeating(nameof(TriggerScream), screamInterval, screamInterval);
    }

    void TriggerScream()
    {
        // If dead, stop screaming and cancel future invokes
        if (healthSystem != null && healthSystem.IsDead)
        {
            CancelInvoke(nameof(TriggerScream));
            return;
        }

        if (animator != null)
            animator.SetTrigger("Scream");
    }
}
