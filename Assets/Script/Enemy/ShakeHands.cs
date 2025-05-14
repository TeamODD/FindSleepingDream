using UnityEngine;

public class ShakeHands : MonoBehaviour
{
    public float shakeAmount = 0.05f;
    public float shakeSpeed = 10f;
    private Vector3 originalLocalPosition;

    void Start()
    {
        // ✅ 부모 기준 상대 위치 저장
        originalLocalPosition = transform.localPosition;
    }

    void LateUpdate()
    {
        float shakeX = Mathf.Sin(Time.time * shakeSpeed) * shakeAmount;
        float shakeY = Mathf.Cos(Time.time * shakeSpeed) * shakeAmount;

        // ✅ 부모 기준 위치에 흔들림만 더함
        transform.localPosition = originalLocalPosition + new Vector3(shakeX, shakeY, 0f);
    }
}
