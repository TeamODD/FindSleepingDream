using UnityEngine;

public class MonsterDummy : MonoBehaviour
{
    public Transform target; // 보통 플레이어를 드래그해서 넣음
    public float moveSpeed = 1.5f; // 주인공이 3이면 1.5로 설정
    private bool isStunned = false;
    private float stunTimer = 0f;

    private void Update()
    {
        if (isStunned)
        {
            stunTimer -= Time.deltaTime;
            if (stunTimer <= 0f)
            {
                isStunned = false;
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

    public void Stun()
    {
        isStunned = true;
        stunTimer = 1f;
        Debug.Log("[Monster] 스턴됨 (1초)");
    }
}
