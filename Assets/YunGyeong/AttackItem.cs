using System.Threading;
using UnityEngine;

public class AttackItem : MonoBehaviour
{
    public Transform target; // 타겟
    public float delayBeforeThrow = 0.5f; // 잠깐 숨기고 날아가기 전 딜레이
    public float throwForce = 15f;
    public float arcHeight = 15f; // 포물선 높이 조절용
    private bool isThrown = false;

    private SpriteRenderer sr;
    private Rigidbody2D rb;
    private Collider2D col;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }

    public void StartThrowToTarget()
    {
        if (isThrown || target == null) return;
        isThrown = true;
        StartCoroutine(ThrowRoutine());
    }

    private System.Collections.IEnumerator ThrowRoutine()
    {
        // 1. 기물 숨기기
        sr.enabled = false;
        col.enabled = false;
        rb.bodyType = RigidbodyType2D.Kinematic;
        yield return new WaitForSeconds(delayBeforeThrow);

        // 2. 위치 유지하면서 다시 보이기
        sr.enabled = true;
        col.enabled = true;
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.linearVelocity = Vector2.zero;

        // 3. 포물선 계산
        Vector2 dir = (target.position - transform.position);
        float distance = dir.magnitude;

        float vx = dir.x * throwForce / distance;
        float vy = arcHeight;

        Vector2 launchVelocity = new Vector2(vx, vy);
        rb.AddForce(launchVelocity, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster"))
        {
            collision.GetComponent<MonsterDummy>()?.Stun();
            Destroy(gameObject); // 닿으면 완전 삭제!
        }
    }
}
