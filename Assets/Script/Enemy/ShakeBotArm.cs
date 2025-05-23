using UnityEngine;

public class ShakeBotArm : MonoBehaviour
{
    public float shakeAmount = 0.05f;   // 흔들림 세기
    public float shakeSpeed = 10f;      // 흔들림 속도
    private Vector3 originalPosition;

    void Start()
    {
        originalPosition = transform.localPosition;  // 초기 위치 저장
    }

    void Update()
    {
        float shakeX = Mathf.Sin(Time.time * shakeSpeed) * shakeAmount;
        float shakeY = Mathf.Cos(Time.time * shakeSpeed) * shakeAmount;
        transform.localPosition = originalPosition + new Vector3(0f, shakeY, 0f);
    }
}