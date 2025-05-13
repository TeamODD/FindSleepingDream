//using UnityEngine;
//using System.Collections;

//public class SpawnEnemy : MonoBehaviour
//{
//    public GameObject enemyprefab;

//    public float spawnCoolDown = 3f;

//    public float deadcooldown = 10f;

//    private void Start()
//    {
//        StartCoroutine(Spawn());
//        rb = GetComponent<Rigidbody2D>();
//        moveEnemy = GetComponent<MoveEnemy>();
//    }

//    private IEnumerator Spawn()
//    {
//        while (true)
//        {
//            Destroy(Instantiate(enemyprefab, transform.position, Quaternion.identity), deadcooldown); ;

//            yield return new WaitForSeconds(spawnCoolDown);
//        }

//    }
//    public Transform player;
//    public float playerDetectRange = 5f;
//    public float throwForce = 10f;
//    public float throwCheckRadius = 1f;

//    private Rigidbody2D rb;
//    private bool isChasing = false;

//    private MoveEnemy moveEnemy;


//    void Update()
//    {
//        DetectPlayer();

//        if (isChasing)
//        {
//            ChasePlayer();
//            CheckAndThrowObstacle();
//        }
//        else
//        {
//            rb.linearVelocity = Vector2.zero;
//        }
//    }

//    void DetectPlayer()
//    {
//        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
//        isChasing = distanceToPlayer <= playerDetectRange;
//    }

//    void ChasePlayer()
//    {
//        Vector2 dir = (player.position - transform.position).normalized;

//        // MoveEnemy���� �������� ���ϴ� moveSpeed ���!
//        rb.linearVelocity = dir * MoveEnemy.CurrentSpeed;
//    }

//    void CheckAndThrowObstacle()
//    {
//        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, throwCheckRadius);
//        foreach (var hit in hits)
//        {
//            if (hit.CompareTag("Obstacle"))
//            {
//                ThrowObject(hit.gameObject);
//                break;
//            }
//        }
//    }

//    void ThrowObject(GameObject obstacle)
//    {
//        Rigidbody2D rbObstacle = obstacle.GetComponent<Rigidbody2D>();
//        if (rbObstacle != null)
//        {
//            rbObstacle.isKinematic = false;

//            // �����ٴ� ǥ�� ���̱�
//            if (obstacle.GetComponent<Highlight>() == null)
//            {
//                obstacle.AddComponent<Highlight>();
//            }

//            Vector2 playerTop = (Vector2)player.position + Vector2.up * 0.5f;
//            Vector2 playerBot = (Vector2)player.position + Vector2.down * 0.5f;
//            Vector2[] targets = { playerTop, playerBot };
//            Vector2 targetPos = targets[Random.Range(0, targets.Length)];
//            Vector2 throwDir = (targetPos - (Vector2)obstacle.transform.position).normalized;

//            rbObstacle.AddForce(throwDir * throwForce, ForceMode2D.Impulse);
//        }

//        Destroy(obstacle, 3f);
//    }

//    void OnDrawGizmosSelected()
//    {
//        Gizmos.color = Color.red;
//        Gizmos.DrawWireSphere(transform.position, playerDetectRange);
//        Gizmos.color = Color.blue;
//        Gizmos.DrawWireSphere(transform.position, throwCheckRadius);
//    }
//}


using UnityEngine;
using System.Collections;

public class SpawnEnemy : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject enemyprefab;
    public float spawnCoolDown = 3f; // 스폰 쿨타임
    public float deadcooldown = 10f;

    [Header("Chase & Throw Settings")]
    public Transform player;
    public float playerDetectRange = 5f;
    public float throwForce = 10f;
    public float throwCheckRadius = 1f;

    private Rigidbody2D rb;
    private MoveEnemy moveEnemy;
    private bool isChasing = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        moveEnemy = GetComponent<MoveEnemy>();
        StartCoroutine(Spawn());
    }

    private void Update()
    {
        DetectPlayer();

        if (isChasing)
        {
            //ChasePlayer();
            CheckAndThrowObstacle();
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    private IEnumerator Spawn()
    {
        while (true)
        {
            GameObject enemy = Instantiate(enemyprefab, transform.position, Quaternion.identity);
            Destroy(enemy, deadcooldown);

            yield return new WaitForSeconds(spawnCoolDown);
        }
    }

    private void DetectPlayer()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        isChasing = distanceToPlayer <= playerDetectRange;
    }

    //private void ChasePlayer()
    //{
    //    Vector2 dir = (player.position - transform.position).normalized;
    //    rb.linearVelocity = dir * MoveEnemy.CurrentSpeed;
    //}

    private void CheckAndThrowObstacle()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, throwCheckRadius);

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Obstacle"))
            {
                ThrowObject(hit.gameObject);
                break; // �� ���� �ϳ��� ������
            }
        }
    }

    private void ThrowObject(GameObject obstacle)
    {
        Rigidbody2D rbObstacle = obstacle.GetComponent<Rigidbody2D>();
        if (rbObstacle == null) return;

        rbObstacle.isKinematic = false;

        // �̹� ���� ������Ʈ�� ����
        if (obstacle.GetComponent<Highlight>() == null)
        {
            obstacle.AddComponent<Highlight>();
        }

        // �÷��̾� ��/�Ʒ� �� ������ ��ġ�� ��ǥ��
        Vector2 playerTop = (Vector2)player.position + Vector2.up * 0.2f;
        Vector2 playerBot = (Vector2)player.position + Vector2.down * 0.5f;
        Vector2[] targets = { playerTop, playerBot };

        Vector2 targetPos = targets[Random.Range(0, targets.Length)];
        Vector2 throwDir = (targetPos - (Vector2)transform.position).normalized;

        rbObstacle.AddForce(throwDir * throwForce, ForceMode2D.Impulse);

        Destroy(obstacle, 3f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerMode>()?.Die();
            Destroy(gameObject);


        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, playerDetectRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, throwCheckRadius);
    }
}