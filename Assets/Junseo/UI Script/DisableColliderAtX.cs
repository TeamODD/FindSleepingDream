using UnityEngine;

public class DisableColliderAtX : MonoBehaviour
{
    public float targetX = 10f; // 도달해야 할 x 좌표값
    private BoxCollider2D boxCol;
    private bool isDisabled = false;

    void Start()
    {
        boxCol = GetComponent<BoxCollider2D>();
        if (boxCol == null)
        {
            Debug.LogWarning("BoxCollider2D가 이 오브젝트에 없습니다.");
        }
    }

    void Update()
    {
        if (!isDisabled && transform.position.x >= targetX)
        {
            if (boxCol != null)
            {
                boxCol.enabled = false;
                isDisabled = true;
                Debug.Log("BoxCollider2D 비활성화됨!");
            }
        }
    }
}
