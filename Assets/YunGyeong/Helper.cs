using UnityEngine;

public class Helper : MonoBehaviour
{
    [Tooltip("이동 속도")]
    public float moveSpeed = 4f;
    [Tooltip("앞으로 이동할 방향")]
    public Vector2 moveDirection = Vector2.right;

    [Tooltip("플레이어가 따라오지 않는다고 판단할 거리")]
    public float maxDistanceToPlayer = 8f;
    [Tooltip("되돌아가서 기다리는 거리")]
    public float returnDistance = 3f;
    [Tooltip("플레이어가 다시 움직였다고 판단할 최소 속도")]
    public float playerMoveThreshold = 0.1f;

    private Rigidbody2D rb;
    private Animator animator;
    private Transform playerTransform;
    private Rigidbody2D playerRb;

    private enum State { Forward, Returning }
    private State currentState = State.Forward;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

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

        switch (currentState)
        {
            case State.Forward:
                // 플레이어가 안 따라오고 멈춰있으면 돌아감
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
                // 플레이어가 다시 움직이기 시작하면 다시 앞으로 감
                if (playerSpeed >= playerMoveThreshold)
                {
                    currentState = State.Forward;
                }
                else
                {
                    // 플레이어에게 다가감
                    Vector2 dirToPlayer = (playerTransform.position - transform.position).normalized;
                    MoveInDirection(dirToPlayer);

                    // 너무 가까워지면 멈춤
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
        if (animator != null) animator.SetBool("IsWalking", true);
    }
}
