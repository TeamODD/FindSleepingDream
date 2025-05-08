using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D), typeof(SpriteRenderer))]
public class AttackItem : MonoBehaviour
{
    [Header("기본 설정")]
    public float speed = 15f;
    public float activeDuration = 5f; // 활성화 후 자동 소멸 시간 (선택 사항)

    [Header("타겟 설정")]
    public bool targetNearestMonster = true; // 가장 가까운 몬스터 자동 타겟팅 여부
    public Transform manualTarget; // 수동으로 설정할 타겟 (자동 타겟팅 비활성화 시 사용)
    public bool flyLeftIfNoTarget = false; // 타겟 없을 시 왼쪽으로 날아갈지 여부

    private Rigidbody2D rb;
    private Collider2D col;
    private SpriteRenderer sr;
    private bool isActive = false;
    private Transform target;
    private bool isUsed = false;
    public bool IsUsed => isUsed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();

        rb.bodyType = RigidbodyType2D.Kinematic; // 처음엔 물리 영향 받지 않음
        rb.gravityScale = 0f;
        col.isTrigger = true; // 처음엔 트리거 상태
    }

    private void Update()
    {
        if (isActive) return; // 이미 활성화된 아이템은 더 이상 입력 감지 안 함
    }

    public void Activate()
    {
        if (isActive || isUsed) return;
        isActive = true;
        isUsed = true;

        rb.bodyType = RigidbodyType2D.Dynamic; // 물리 활성화
        col.isTrigger = false; // 충돌 감지 활성화

        // 타겟 설정
        if (targetNearestMonster)
        {
            target = FindNearestMonster();
        }
        else
        {
            target = manualTarget;
        }

        // 이동 방향 설정
        Vector2 direction;
        if (target != null)
        {
            direction = (target.position - transform.position).normalized;
        }
        else if (flyLeftIfNoTarget)
        {
            direction = Vector2.left;
        }
        else
        {
            Debug.LogWarning("[AttackItem] 활성화되었지만 타겟이 없어 움직이지 않습니다.");
            rb.linearVelocity = Vector2.zero;
            return;
        }

        rb.linearVelocity = direction * speed;

        // 활성화 후 자동 소멸 (선택 사항)
        if (activeDuration > 0)
        {
            Destroy(gameObject, activeDuration);
        }
    }

    private Transform FindNearestMonster()
    {
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
        Transform nearest = null;
        float minDist = Mathf.Infinity;

        foreach (GameObject m in monsters)
        {
            float dist = Vector2.Distance(transform.position, m.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                nearest = m.transform;
            }
        }
        return nearest;
    }

    private void OnCollisionEnter2D(Collision2D collision) // 트리거 충돌 대신 물리 충돌 감지
    {
        if (isActive && collision.gameObject.CompareTag("Monster"))
        {
            MonsterDummy monster = collision.gameObject.GetComponent<MonsterDummy>();
            if (monster != null)
            {
                monster.Stun(1f); // 1초 스턴
            }
            Destroy(gameObject); // 몬스터에 닿으면 즉시 사라짐
        }
        // 몬스터가 아닌 다른 오브젝트와 충돌했을 때의 처리 (선택 사항)
        else if (isActive && !collision.gameObject.CompareTag("Monster") && !collision.collider.isTrigger)
        {
            Destroy(gameObject);
        }
    }
}