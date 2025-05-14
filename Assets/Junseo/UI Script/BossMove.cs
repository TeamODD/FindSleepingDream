using UnityEngine;

public class BossMove : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float moveDelay = 1f;
    public float endX = 168f;

    private float timer = 0f;
    private bool shouldMove = false;

    void OnEnable()
    {
        timer = 0f;
        shouldMove = false;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (!shouldMove && timer >= moveDelay)
        {
            shouldMove = true;
            Debug.Log("🟢 이동 시작!");
        }

        if (shouldMove)
        {
            // X값 증가
            float newX = transform.position.x + moveSpeed * Time.deltaTime;

            // 목표 위치 넘지 않도록 Clamp
            if (newX >= endX)
            {
                newX = endX;
                shouldMove = false;  // ✅ 멈추기만 하고 사라지지 않음
                Debug.Log("🛑 도달: 이동 멈춤");
            }

            transform.position = new Vector3(newX, transform.position.y, transform.position.z);
        }
    }
}
