using UnityEngine;

public class Test_Throw : MonoBehaviour
{
    public Transform player;
    public float playerDetectRange = 5f;
    public float throwForce = 10f;
    public float throwCheckRadius = 1f;

    private Rigidbody2D rb;
    private bool isChasing = false;

    private MoveEnemy moveEnemy;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        moveEnemy = GetComponent<MoveEnemy>();
    }

    void Update()
    {
        DetectPlayer();

        if (isChasing)
        {
            ChasePlayer();
            CheckAndThrowObstacle();
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    void DetectPlayer()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        isChasing = distanceToPlayer <= playerDetectRange;
    }

    void ChasePlayer()
    {
        Vector2 dir = (player.position - transform.position).normalized;

        // MoveEnemy에서 동적으로 변하는 moveSpeed 사용!
        rb.linearVelocity = dir * MoveEnemy.CurrentSpeed;
    }

    void CheckAndThrowObstacle()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, throwCheckRadius);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Obstacle"))
            {
                ThrowObject(hit.gameObject);
                break;
            }
        }
    }

    void ThrowObject(GameObject obstacle)
    {
        Rigidbody2D rbObstacle = obstacle.GetComponent<Rigidbody2D>();
        if (rbObstacle != null)
        {
            rbObstacle.isKinematic = false;

            // 던졌다는 표시 붙이기
            if (obstacle.GetComponent<Highlight>() == null)
            {
                obstacle.AddComponent<Highlight>();
            }

            Vector2 playerTop = (Vector2)player.position + Vector2.up * 0.5f;
            Vector2 playerBot = (Vector2)player.position + Vector2.down * 0.5f;
            Vector2[] targets = { playerTop, playerBot };
            Vector2 targetPos = targets[Random.Range(0, targets.Length)];
            Vector2 throwDir = (targetPos - (Vector2)obstacle.transform.position).normalized;

            rbObstacle.AddForce(throwDir * throwForce, ForceMode2D.Impulse);
        }

        Destroy(obstacle, 3f);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, playerDetectRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, throwCheckRadius);
    }
}