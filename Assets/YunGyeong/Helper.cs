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

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer; // ✅ 추가
    private Transform playerTransform;
    private Rigidbody2D playerRb;

    private enum State { Forward, Returning }
    private State currentState = State.Forward;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // ✅ 추가

        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
            rb.gravityScale = 0f;
        }

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
        if (playerTransform == null || playerRb == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
        float playerSpeed = Mathf.Abs(playerRb.linearVelocity.x);
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);

        switch (currentState)
        {
            case State.Forward:
                if (distanceToPlayer > maxDistanceToPlayer && playerSpeed < playerMoveThreshold)
                {
                    currentState = State.Returning;
                }
                else
                {
                    MoveInDirection(moveDirection);
                }
                break;

            case State.Returning:
                if (playerSpeed >= playerMoveThreshold)
                {
                    currentState = State.Forward;
                }
                else
                {
                    Vector2 dirToPlayer = (playerTransform.position - transform.position).normalized;
                    MoveInDirection(dirToPlayer);

                    if (distanceToPlayer < returnDistance)
                    {
                        rb.linearVelocity = Vector2.zero;
                        if (animator != null) animator.SetBool("IsWalking", false);
                    }
                }
                break;
        }
    }

    private void MoveInDirection(Vector2 dir)
    {
        rb.linearVelocity = dir.normalized * moveSpeed;

        if (animator != null)
            animator.SetBool("IsWalking", true);

        // ✅ Sprite 방향에 따라 반전 처리
        if (spriteRenderer != null && dir.x != 0)
            spriteRenderer.flipX = dir.x > 0;
    }
}
