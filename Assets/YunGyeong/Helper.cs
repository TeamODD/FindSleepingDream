using UnityEngine;

public class Helper : MonoBehaviour
{
    [Tooltip("이동 속도")]
    public float moveSpeed = 2.8f;
    [Tooltip("앞으로 이동할 방향")]
    public Vector2 moveDirection = Vector2.right;

    [Tooltip("플레이어가 따라오지 않는다고 판단할 거리")]
    public float maxDistanceToPlayer = 6f;
    [Tooltip("되돌아가서 기다리는 거리")]
    public float returnDistance = 12f;
    [Tooltip("플레이어가 다시 움직였다고 판단할 최소 속도")]
    public float playerMoveThreshold = 0.1f;

    private enum State { Forward, ReturningWait, Returning }
    private State currentState = State.Forward;

    public float waitTime = 3f; // 처음 대기 시간
    private float startTime;
    private float waitTimer = 0f;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Transform playerTransform;
    private Rigidbody2D playerRb;

    void Start()
    {
        startTime = Time.time;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
            rb.gravityScale = 0f;
        }
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.gravityScale = 0f;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
            playerRb = player.GetComponent<Rigidbody2D>();
        }
        else
        {
            Debug.LogError("Helper: 'Player' 태그를 가진 오브젝트를 찾을 수 없습니다!");
            enabled = false;
        }
    }

    void FixedUpdate()
    {
        if (Time.time - startTime < waitTime)
        {
            if (animator != null) animator.SetBool("IsWalking", false);
            rb.linearVelocity = Vector2.zero;
            return;
        }

        if (playerTransform == null || playerRb == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
        float playerMoveDir = playerRb.linearVelocity.x;

        bool isPlayerFollowing =
            Mathf.Abs(playerMoveDir) >= playerMoveThreshold &&
            Mathf.Sign(playerMoveDir) == Mathf.Sign(moveDirection.x);

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);  // y 고정

        switch (currentState)
        {
            case State.Forward:
                if (!isPlayerFollowing && distanceToPlayer > maxDistanceToPlayer)
                {
                    FlipDirectionToPlayer();
                    waitTimer = 0f;
                    currentState = State.ReturningWait;
                }
                else
                {
                    MoveInDirection(moveDirection);
                }
                break;

            case State.ReturningWait:
                if (isPlayerFollowing)
                {
                    currentState = State.Forward;
                    break;
                }

                waitTimer += Time.fixedDeltaTime;
                if (waitTimer >= 3f)
                {
                    currentState = State.Returning;
                }

                rb.linearVelocity = Vector2.zero;
                if (animator != null) animator.SetBool("IsWalking", false);
                break;

            case State.Returning:
                if (isPlayerFollowing)
                {
                    currentState = State.Forward;
                    break;
                }

                Vector2 dirToPlayer = (playerTransform.position - transform.position).normalized;
                MoveInDirection(dirToPlayer);

                if (distanceToPlayer < returnDistance)
                {
                    rb.linearVelocity = Vector2.zero;
                    if (animator != null) animator.SetBool("IsWalking", false);
                }
                break;
        }
        //플레이어가 일정 좌표 지나갈 시에 조력자 파괴
        if (playerTransform != null && playerTransform.position.x >= 89.3f)
        {
            Destroy(gameObject);
        }
    }

    private void MoveInDirection(Vector2 dir)
    {
        rb.linearVelocity = dir.normalized * moveSpeed;
        if (animator != null) animator.SetBool("IsWalking", true);
        if (spriteRenderer != null && dir.x != 0)
            spriteRenderer.flipX = dir.x > 0;
    }

    private void FlipDirectionToPlayer()
    {
        if (spriteRenderer != null && playerTransform != null)
        {
            spriteRenderer.flipX = (playerTransform.position - transform.position).x > 0;
        }
    }
}
