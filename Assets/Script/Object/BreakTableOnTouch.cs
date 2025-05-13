using UnityEngine;

public class BreakableTable : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log($"{gameObject.name}��(��) ���� �浹�Ͽ� ���ŵ�!");
            Destroy(gameObject);
        }
    }
}