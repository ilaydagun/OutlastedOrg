using System.Collections;
using UnityEngine;

public class ZombieMeleeAttackTimed : MonoBehaviour
{
    public string targetTag = "Player";
    public float damageAmount = 15f;

    public Animator animator;
    public string attackTriggerName = "Attack";
    public AnimationClip attackClip;

    [Header("Hit Timing")]
    [Range(0.1f, 0.9f)]
    public float hitTimePercent = 0.45f;

    public bool useAnimationEvent = false;

    public float attackRange = 1.6f;
    public Transform zombieCenter;

    private Transform target;
    private HealthSystem targetHealth;
    private Coroutine routine;
    private bool canDealDamage = false;

    private HealthSystem selfHealth;

    private void Awake()
    {
        selfHealth = GetComponent<HealthSystem>();
        if (animator == null) animator = GetComponent<Animator>();
    }

    private void OnDisable()
    {
        // If HealthSystem disables this script on death, this ensures the coroutine stops too
        StopAttacking();
    }

    private float ClipLen() => attackClip != null ? Mathf.Max(0.05f, attackClip.length) : 1f;

    private void OnTriggerEnter(Collider other)
    {
        if (selfHealth != null && selfHealth.IsDead) return;
        if (!other.CompareTag(targetTag)) return;

        target = other.transform;
        targetHealth = other.GetComponent<HealthSystem>();
        if (targetHealth == null) return;

        // Extra safety: if target is dead, do not start loop
        if (targetHealth.IsDead) return;

        if (routine == null)
            routine = StartCoroutine(AttackLoop());
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(targetTag)) return;
        StopAttacking();
    }

    private void StopAttacking()
    {
        target = null;
        targetHealth = null;

        if (routine != null)
        {
            StopCoroutine(routine);
            routine = null;
        }
    }

    private bool IsInRange()
    {
        if (target == null) return false;
        Vector3 from = zombieCenter != null ? zombieCenter.position : transform.position;
        return Vector3.Distance(from, target.position) <= attackRange;
    }

    private IEnumerator AttackLoop()
    {
        while (true)
        {
            // Zombie dead or target missing/dead => stop
            if (selfHealth != null && selfHealth.IsDead) { StopAttacking(); yield break; }
            if (targetHealth == null || targetHealth.IsDead) { StopAttacking(); yield break; }

            // Not in range => wait a frame
            if (!IsInRange())
            {
                yield return null;
                continue;
            }

            float len = ClipLen();
            float ht = Mathf.Clamp(len * hitTimePercent, 0.1f, len - 0.1f);

            canDealDamage = false;

            if (animator != null)
                animator.SetTrigger(attackTriggerName);

            if (useAnimationEvent)
            {
                float waitTime = 0f;
                while (!canDealDamage && waitTime < len)
                {
                    if (selfHealth != null && selfHealth.IsDead) { StopAttacking(); yield break; }
                    if (targetHealth == null || targetHealth.IsDead) { StopAttacking(); yield break; }

                    waitTime += Time.deltaTime;
                    yield return null;
                }

                float rest = len - waitTime;
                if (rest > 0f) yield return new WaitForSeconds(rest);
            }
            else
            {
                yield return new WaitForSeconds(ht);

                if (selfHealth != null && selfHealth.IsDead) { StopAttacking(); yield break; }
                if (targetHealth == null || targetHealth.IsDead) { StopAttacking(); yield break; }

                if (IsInRange())
                    targetHealth.TakeDamage(damageAmount);

                float rest = len - ht;
                if (rest > 0f) yield return new WaitForSeconds(rest);
            }
        }
    }

    // Animation Event
    public void OnAttackHit()
    {
        // If zombie died, ignore event completely
        if (selfHealth != null && selfHealth.IsDead) return;

        canDealDamage = true;

        if (targetHealth != null && !targetHealth.IsDead && IsInRange())
            targetHealth.TakeDamage(damageAmount);
    }
}
