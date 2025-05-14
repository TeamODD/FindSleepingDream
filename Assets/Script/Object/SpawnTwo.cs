using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpawnTwo : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject[] enemyprefab;
    public float spawnCoolDown = 3f; // ���� ��Ÿ��
    public float deadcooldown = 10f;

    [Header("Chase & Throw Settings")]
    public Transform player;
    public float playerDetectRange = 5f;
    public float throwForce = 10f;
    public float throwCheckRadius = 1f;
    public LayerMask ObstacleLayer;

    private Rigidbody2D rb;
    private MoveEnemy moveEnemy;
    private bool isChasing = false;


    private float throwCooldown = 2f;
    private float lastThrowTime;



    private IEnumerator Spawn()
    {
        while (true)
        {
            int index = Random.Range(0, enemyprefab.Length);
            GameObject enemy=Instantiate(enemyprefab[index],transform.position,Quaternion.identity);
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



    //private void OnCollisionEnter2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Player"))
    //    {
    //        collision.gameObject.GetComponent<PlayerMove>()?.Die();
    //        Destroy(gameObject);
    //        Debug.Log("die �Լ� ȣ��");


    //    }
    //}

    // ������Ʈ ������ ����
    private void ThrowObject(GameObject obstacle)
    {
        
        Rigidbody2D rbObstacle = obstacle.GetComponent<Rigidbody2D>();
        if (rbObstacle == null) return;
        rbObstacle.bodyType = RigidbodyType2D.Dynamic;

        Vector2 playerTop = (Vector2)player.position + Vector2.up * 0.2f; // �÷��̾� �Ӹ��� �ٸ� ���
        Vector2 playermid = (Vector2)player.position + Vector2.up * 0.4f; // ������ �����ʰ� 
        Vector2 playerBot = (Vector2)player.position + Vector2.down * 0.4f; // �ٸ������ؼ� ���� �� �ְ�
        Vector2[] targets = { playerTop, playerBot };

        Vector2 targetPos = targets[Random.Range(0, targets.Length)];
        Vector2 throwDir = (targetPos - (Vector2)transform.position).normalized;

        rbObstacle.AddForce(throwDir * throwForce, ForceMode2D.Impulse); // ������ ��

        Destroy(obstacle, 2f);



    }



    // �÷��̾� ���� ��, ������Ʈ ������ �Լ�.
    private void CheckAndThrowObstacle()
    {
        var hits = Physics2D.OverlapCircleAll(transform.position, throwCheckRadius, ObstacleLayer);
        // ��������.
        // overlap : ������ ���� ���ο� ���ϴ� ������Ʈ�� ã�� �� �ִ� ���.
        // ���׶� ������ ���ϸ� Circle. �پ��� ���������� ����.
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Obstacle"))
            {
                ThrowObject(hit.gameObject);
                break;
            }
        }
        


    }

        void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        moveEnemy = GetComponent<MoveEnemy>();
        StartCoroutine(Spawn());
    }

    void Update()
    {
        DetectPlayer();

        if (isChasing)
        {
            CheckAndThrowObstacle(); 
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
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
