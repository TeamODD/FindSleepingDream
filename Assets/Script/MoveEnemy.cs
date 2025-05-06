using UnityEngine;

public class MoveEnemy : MonoBehaviour
{
    public float minSpeed = 1f;
    public float maxSpeed = 4f;
    public static float CurrentSpeed { get; private set; }

    private float changeInterval;
    private float timer;

    private bool isChasing = false;  // 추격 중인지 외부에서 설정 가능하게

    void Start()
    {
        SetRandomSpeed();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= changeInterval)
        {
            SetRandomSpeed();
        }

        if (!isChasing)
        {
            // 플레이어를 쫓아가지 않을 때는 이동 X (그 자리 유지)
            return;
        }

        // 쫓아가는 쪽 로직은 다른 스크립트에서 Rigidbody2D.velocity로 처리
    }

    void SetRandomSpeed()
    {
        CurrentSpeed = Random.Range(minSpeed, maxSpeed);
        changeInterval = Random.Range(2f, 5f);
        timer = 0f;

        Debug.Log("Speed changed to: " + CurrentSpeed);
    }

    // 외부에서 호출
    public void SetChasing(bool value)
    {
        isChasing = value;
    }
}