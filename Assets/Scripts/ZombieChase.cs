using UnityEngine;

public class ZombieChase : MonoBehaviour
{
    public Transform player;

    [Header("RANGES")]
    public float detectionRadius = 8f;
    public float attackRange = 1.6f;

    [Header("SPEEDS")]
    public float walkSpeed = 1.2f;
    public float runSpeed = 3.2f;
    public float rotateSpeed = 720f;

    [Header("WANDER")]
    public float wanderRadius = 6f;
    public float wanderChangeInterval = 2.5f;
    public float wanderStopChance = 0.15f; // sometimes pause instead of walking
    public float wanderStopTime = 1.0f;

    [Header("COLLISION")]
    [Tooltip("Zombiler üst üste binmesin diye CharacterController ile hareket eder. Prefab'a CharacterController ekle.")]
    public bool useCharacterController = true;

    private Animator animator;
    private HealthSystem healthSystem;
    private CharacterController cc;

    private Vector3 wanderTarget;
    private float nextWanderChangeTime;
    private float stopUntilTime;

    void Start()
    {
        animator = GetComponent<Animator>();
        healthSystem = GetComponent<HealthSystem>();
        cc = GetComponent<CharacterController>();

        PickNewWanderTarget();
        nextWanderChangeTime = Time.time + wanderChangeInterval;
    }

    void Update()
    {
        if (healthSystem != null && healthSystem.IsDead) return;

        // If we want CC movement but there is no CC, fallback to transform movement
        if (useCharacterController && cc == null)
            cc = GetComponent<CharacterController>();

        if (player == null) { Wander(); return; }

        float distance = Vector3.Distance(transform.position, player.position);

        // If player not detected, wander
        if (distance > detectionRadius)
        {
            Wander();
            return;
        }

        // If in attack range: stop and face player
        if (distance <= attackRange)
        {
            SetIdleAttackFacing();
            FacePlayer();
            return;
        }

        // Player detected: RUN chase
        ChasePlayer(runSpeed);
    }

    private void Wander()
    {
        // Occasionally stop
        if (Time.time < stopUntilTime)
        {
            SetAnimIdle();
            return;
        }

        if (Time.time >= nextWanderChangeTime)
        {
            // Random pause chance
            if (Random.value < wanderStopChance)
            {
                stopUntilTime = Time.time + wanderStopTime;
                SetAnimIdle();
                nextWanderChangeTime = Time.time + wanderChangeInterval;
                return;
            }

            PickNewWanderTarget();
            nextWanderChangeTime = Time.time + wanderChangeInterval;
        }

        MoveTowards(wanderTarget, walkSpeed);
        SetAnimWalk();
    }

    private void PickNewWanderTarget()
    {
        Vector2 rand = Random.insideUnitCircle * wanderRadius;
        wanderTarget = new Vector3(transform.position.x + rand.x, transform.position.y, transform.position.z + rand.y);
    }

    private void ChasePlayer(float speed)
    {
        Vector3 targetPos = player.position;
        targetPos.y = transform.position.y;

        MoveTowards(targetPos, speed);
        SetAnimRun();
    }

    private void MoveTowards(Vector3 targetPos, float speed)
    {
        Vector3 direction = (targetPos - transform.position);
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.2f) return;

        direction.Normalize();

        // Move with CharacterController to collide with other zombies (prevents stacking)
        if (useCharacterController && cc != null && cc.enabled)
        {
            cc.Move(direction * speed * Time.deltaTime);
        }
        else
        {
            // Fallback (can cause stacking because it bypasses physics)
            transform.position += direction * speed * Time.deltaTime;
        }

        Quaternion targetRot = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, rotateSpeed * Time.deltaTime);
    }

    private void FacePlayer()
    {
        Vector3 direction = (player.position - transform.position);
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.0001f) return;

        direction.Normalize();
        Quaternion targetRot = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, rotateSpeed * Time.deltaTime);
    }

    private void SetIdleAttackFacing()
    {
        SetAnimIdle();
        if (HasParam("isAttacking")) animator.SetBool("isAttacking", true);
    }

    private void SetAnimIdle()
    {
        if (animator == null) return;
        animator.SetBool("isWalking", false);
        if (HasParam("isRunning")) animator.SetBool("isRunning", false);
        if (HasParam("isAttacking")) animator.SetBool("isAttacking", false);
    }

    private void SetAnimWalk()
    {
        if (animator == null) return;
        animator.SetBool("isWalking", true);
        if (HasParam("isRunning")) animator.SetBool("isRunning", false);
        if (HasParam("isAttacking")) animator.SetBool("isAttacking", false);
    }

    private void SetAnimRun()
    {
        if (animator == null) return;
        animator.SetBool("isWalking", false);
        if (HasParam("isRunning")) animator.SetBool("isRunning", true);
        if (HasParam("isAttacking")) animator.SetBool("isAttacking", false);
    }

    private bool HasParam(string paramName)
    {
        if (animator == null) return false;
        foreach (var p in animator.parameters)
            if (p.name == paramName) return true;
        return false;
    }
}
