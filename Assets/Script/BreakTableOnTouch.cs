using UnityEngine;

public class BreakableTable : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log($"{gameObject.name}이(가) 적과 충돌하여 제거됨!");
            Destroy(gameObject);
        }
    }
}