using UnityEngine;

public class Helper : MonoBehaviour
{
    [Tooltip("이동 속도")]
    public float moveSpeed = 1f;
    [Tooltip("이동 방향 (Vector2)")]
    public Vector2 moveDirection = Vector2.right; // 기본적으로 오른쪽으로 이동

    [Tooltip("플레이어를 감지할 최대 거리 (이 거리 이상 멀어지면 멈춤)")]
    public float maxDistanceToPlayer = 8f;
    [Tooltip("플레이어가 다시 따라올 거리 (이 거리 이하로 가까워지면 다시 이동)")]
    public float minDistanceToPlayer = 5f;

    private Rigidbody2D rb;
    private Animator animator;
    private Transform playerTransform;
    private bool isWaiting = false; // 현재 기다리는 상태인지 여부

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
            rb.gravityScale = 0f;
        }

        // 시작 시 플레이어 찾기 (Tag로 검색)
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogError("Helper: 'Player' 태그를 가진 오브젝트를 찾을 수 없습니다!");
            enabled = false;
        }
    }

    void FixedUpdate()
    {
        if (playerTransform == null) return;

        // 플레이어와의 거리 계산
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        // 플레이어가 너무 멀리 떨어져 있고, 아직 기다리는 상태가 아니면 멈춤
        if (distanceToPlayer > maxDistanceToPlayer && !isWaiting)
        {
            rb.linearVelocity = Vector2.zero;
            if (animator != null) animator.SetBool("IsWalking", false);
            isWaiting = true;
        }
        // 플레이어가 다시 따라올 거리 안으로 들어오면 다시 이동
        else if (distanceToPlayer <= minDistanceToPlayer && isWaiting)
        {
            rb.linearVelocity = moveDirection.normalized * moveSpeed;
            if (animator != null) animator.SetBool("IsWalking", true);
            isWaiting = false;
        }
        // 그 외의 경우 (플레이어가 적절한 거리에 있으면 계속 이동)
        else if (!isWaiting)
        {
            rb.linearVelocity = moveDirection.normalized * moveSpeed;
            if (animator != null) animator.SetBool("IsWalking", true);
        }
    }

}