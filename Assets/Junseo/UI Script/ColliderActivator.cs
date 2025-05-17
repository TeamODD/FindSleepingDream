using UnityEngine;

public class ColliderActivator : MonoBehaviour
{
    public GameObject targetObject;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            BoxCollider2D wallCollider = targetObject.GetComponent<BoxCollider2D>();
            if (wallCollider != null)
            {
                wallCollider.isTrigger = false;  // 실체화!
                Debug.Log("✅ 벽이 실체화되었습니다!");
            }

            gameObject.SetActive(false);  // 이 트리거는 한 번만 사용
        }
    }
}
