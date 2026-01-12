using UnityEngine;
using UnityEngine.AI;

public class CompanionAI : MonoBehaviour
{
    [Header("Target Settings")]
    public Transform player; // Ana karakter

    [Header("Follow Settings")]
    [SerializeField] private float followDistance = 3f;
    [SerializeField] private float stoppingDistance = 2f;
    [SerializeField] private float updateRate = 0.1f;

    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed = 3.5f;
    [SerializeField] private float runSpeed = 5.5f;
    [SerializeField] private float runDistance = 8f;

    [Header("Look Settings")]
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private bool lookAtPlayer = true;

    private NavMeshAgent agent;
    private float updateTimer;
    private bool isMoving;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // Oyuncuyu otomatik bul
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
            else
            {
                Debug.LogWarning("Player not found! Please assign player or add 'Player' tag to your player object.");
            }
        }

        if (agent != null)
        {
            agent.stoppingDistance = stoppingDistance;
            agent.speed = walkSpeed;
        }
        else
        {
            Debug.LogError("NavMeshAgent component not found! Please add NavMeshAgent to this GameObject.");
        }
    }

    void Update()
    {
        if (player == null || agent == null) return;

        updateTimer += Time.deltaTime;
        if (updateTimer >= updateRate)
        {
            updateTimer = 0f;
            FollowPlayer();
        }
    }

    void FollowPlayer()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer > followDistance)
        {
            isMoving = true;

            // Mesafeye göre hız ayarla
            if (distanceToPlayer > runDistance)
            {
                agent.speed = runSpeed;
            }
            else
            {
                agent.speed = walkSpeed;
            }

            agent.SetDestination(player.position);
        }
        else
        {
            isMoving = false;
            agent.ResetPath();
        }
    }

    void LateUpdate()
    {
        // Duruyorsa oyuncuya bak
        if (lookAtPlayer && player != null && !isMoving)
        {
            Vector3 direction = player.position - transform.position;
            direction.y = 0;

            if (direction.magnitude > 0.1f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            }
        }
    }

    // Karakteri oyuncunun yanına ışınla (acil durumlar için)
    [ContextMenu("Teleport to Player")]
    public void TeleportToPlayer()
    {
        if (player != null)
        {
            agent.Warp(player.position + player.right * 2f);
        }
    }
}