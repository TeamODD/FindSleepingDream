using UnityEngine;

public class Helper : MonoBehaviour
{
    [Tooltip("이동 속도")]
    public float moveSpeed = 4f;
    [Tooltip("이동 방향 (Vector2)")]
    public Vector2 moveDirection = Vector2.right; // 기본적으로 오른쪽으로 이동

    [Tooltip("플레이어를 감지할 최대 거리 (이 거리 이상 멀어지면 멈춤)")]
    public float maxDistanceToPlayer = 8f;
    [Tooltip("플레이어가 다시 따라올 거리 (이 거리 이하로 가까워지면 다시 이동)")]
    public float minDistanceToPlayer = 5f;
    [Tooltip("플레이어를 향해 다가갈 목표 거리")]
    public float approachDistance = 2.5f; // 추가
    [Tooltip("기다리는 시간 (초)")]
    public float waitingTime = 3f; // 추가

    private Rigidbody2D rb;
    private Animator animator;
    private Transform playerTransform;
    private bool isWaiting = false; // 현재 기다리는 상태인지 여부
    private float waitTimer = 0f; // 추가

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

        if (isWaiting)
        {
            waitTimer += Time.fixedDeltaTime;
            if (waitTimer >= waitingTime)
            {
                isWaiting = false;
                waitTimer = 0f;
            }
            rb.linearVelocity = Vector2.zero;
            if (animator != null) animator.SetBool("IsWalking", false);
        }
        // 플레이어가 너무 멀리 떨어져 있고, 아직 기다리는 상태가 아니면 멈춤
        else if (distanceToPlayer > maxDistanceToPlayer)
        {
            rb.linearVelocity = Vector2.zero;
            if (animator != null) animator.SetBool("IsWalking", false);
            isWaiting = true;
            waitTimer = 0f; // 기다리기 시작할 때 타이머 초기화
        }
        // 플레이어가 다시 따라올 거리 안으로 들어오면 다시 이동
        else if (distanceToPlayer > approachDistance) // 목표 거리보다 멀리 있으면 따라감
        {
            Vector2 directionToPlayer = (playerTransform.position - transform.position).normalized;
            rb.linearVelocity = directionToPlayer * moveSpeed;
            if (animator != null) animator.SetBool("IsWalking", true);
        }
        else // 플레이어와 목표 거리 이내에 있으면 멈추고 기다림
        {
            rb.linearVelocity = Vector2.zero;
            if (animator != null) animator.SetBool("IsWalking", false);
            isWaiting = true;
            waitTimer = 0f;
        }
    }
}