using UnityEngine;

public class MonsterDummy : MonoBehaviour
{
    public Transform target; // 보통 플레이어를 드래그해서 넣음
    public float moveSpeed = 1.5f; // 주인공이 3이면 1.5로 설정
    private bool isStunned = false;
    private float stunTimer = 0f;
    private Rigidbody2D rb; // 몬스터의 Rigidbody2D 컴포넌트

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (isStunned)
        {
            stunTimer -= Time.deltaTime;
            if (stunTimer <= 0f)
            {
                isStunned = false;
                // 스턴 해제 시 Rigidbody2D의 bodyType을 Dynamic으로 설정하여 움직임 재개
                if (rb != null)
                {
                    rb.bodyType = RigidbodyType2D.Dynamic;
                }
                Debug.Log("[Monster] 스턴 해제됨");
            }
            return;
        }

        if (target != null)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;
        }
    }

    // 기존 스턴 메서드 (인자 없음) 유지
    public void Stun()
    {
        isStunned = true;
        stunTimer = 1f;
        // 스턴 시 Rigidbody2D의 bodyType을 Kinematic으로 설정하여 움직임 멈춤
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.linearVelocity = Vector2.zero; // 혹시 남아있을 속도도 초기화
        }
        Debug.Log("[Monster] 스턴됨 (1초)");
    }

    // AttackItem에서 호출할 스턴 메서드 (float duration 인자 받음) 추가
    public void Stun(float duration)
    {
        isStunned = true;
        stunTimer = duration;
        // 스턴 시 Rigidbody2D의 bodyType을 Kinematic으로 설정하여 움직임 멈춤
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.linearVelocity = Vector2.zero; // 혹시 남아있을 속도도 초기화
        }
        Debug.Log($"[Monster] {duration}초 동안 스턴됨");
    }

    // 스턴 상태 여부 확인을 위한 public 속성 (선택 사항)
    public bool IsStunned => isStunned;
}