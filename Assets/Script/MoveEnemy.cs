using UnityEngine;

public class MoveEnemy : MonoBehaviour
{
    public float minSpeed = 1f;
    public float maxSpeed = 4f;
    private float moveSpeed;

    private float changeInterval;
    private float timer;

    public float CurrentSpeed => moveSpeed; // ✅ 외부에서 읽을 수 있는 속성

    void Start()
    {
        SetRandomSpeed();
    }

    void Update()
    {
        transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);

        timer += Time.deltaTime;
        if (timer >= changeInterval)
        {
            SetRandomSpeed();
        }
    }

    void SetRandomSpeed()
    {
        moveSpeed = Random.Range(minSpeed, maxSpeed);
        changeInterval = Random.Range(1f, 3f);
        timer = 0f;
        Debug.Log($"[MoveEnemy] 속도 변경: {moveSpeed:F2} (다음 변경까지 {changeInterval:F1}초)");
    }
}