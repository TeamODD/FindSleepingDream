using UnityEngine;

public class ShakeHands: MonoBehaviour
{
    public float shakeAmount = 0.05f;
    public float shakeSpeed = 10f;
    private Vector3 basePosition;

    void Start()
    {
        basePosition = transform.position; // 처음 위치 저장 <- 이걸 해야 제대로 흔들림
    }

    void LateUpdate()
    {
        float shakeX = Mathf.Sin(Time.time * shakeSpeed) * shakeAmount;
        float shakeY = Mathf.Cos(Time.time * shakeSpeed) * shakeAmount;

        transform.position = basePosition + new Vector3(shakeX, shakeY, 0f);
    }
}