using UnityEngine;

public class BossMove : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float moveDelay = 1f;
    public float endX = 168f;

    public float timer = 0f;
    public bool shouldMove = false;

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
            float newX = transform.position.x + moveSpeed * Time.deltaTime;

            if (newX >= endX)
            {
                newX = endX;
                shouldMove = false;
                Debug.Log("🛑 도달: 이동 멈춤");
            }

            transform.position = new Vector3(newX, transform.position.y, transform.position.z);
        }
    }
}
