using UnityEngine;

public class MonsterDummy : MonoBehaviour
{
    public Transform target; // ���� �÷��̾ �巡���ؼ� ����
    public float moveSpeed = 1.5f; // ���ΰ��� 3�̸� 1.5�� ����
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
                Debug.Log("[Monster] ���� ������");
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
        Debug.Log("[Monster] ���ϵ� (1��)");
    }
}
