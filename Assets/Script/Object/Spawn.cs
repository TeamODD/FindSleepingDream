/* 기존 스크립트 작동방식
 1. Spawn 스크립트에서 오브젝트 스폰.
2. 스폰 후 Obstacle 태그 인식 -> Highlight 스크립트 오브젝트에 묻히고 플레이어한테 던지기.
3. 플레이어는 Highlight 가 묻은 오브젝트 인식 후, 스턴

--------> 윤경이랑 깃허브로 합친 후 Highlight 스크립트 작동이 제대로 안돼서..................수정....... 
 */

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


    private void CheckAndThrowObstacle()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, throwCheckRadius); // 물리엔진.
        // overlap : 지정된 범위 내부에 원하는 오브젝트를 찾을 수 있는 기능.
        // 동그란 범위를 원하면 Circle. 다양한 감지모형이 있음.
        // 

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Obstacle"))
            {
                ThrowObject(hit.gameObject);
                break;
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerMove>()?.Die();
            Destroy(gameObject);
            Debug.Log("die 함수 호출");
        }
    }

    private void ThrowObject(GameObject obstacle)
    {
        Rigidbody2D rbObstacle = obstacle.GetComponent<Rigidbody2D>();
        if (rbObstacle == null) return;

        rbObstacle.isKinematic = false;

        //if (obstacle.GetComponent<Highlight>() == null) // 스크립트 붙이기
        //{
        //    obstacle.AddComponent<Highlight>();
        //}

        Vector2 playerTop = (Vector2)player.position + Vector2.up * 0.5f;
        Vector2 playerBot = (Vector2)player.position + Vector2.down * 0.5f;
        Vector2[] targets = { playerTop, playerBot };

        Vector2 targetPos = targets[Random.Range(0, targets.Length)];
        Vector2 throwDir = (targetPos - (Vector2)transform.position).normalized;

        rbObstacle.AddForce(throwDir * throwForce, ForceMode2D.Impulse);

        Destroy(obstacle, 3f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, playerDetectRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, throwCheckRadius);
    }
}