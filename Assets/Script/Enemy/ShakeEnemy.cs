using UnityEngine;

public class ShakeEnemy : MonoBehaviour
{
    public float shakeAmount = 0.05f;   // ��鸲 ����
    public float shakeSpeed = 10f;      // ��鸲 �ӵ�
    private Vector3 originalPosition;

    void Start()
    {
        originalPosition = transform.localPosition;  // �ʱ� ��ġ ����
    }

    void Update()
    {
        float shakeX = Mathf.Sin(Time.time * shakeSpeed) * shakeAmount;
        float shakeY = Mathf.Cos(Time.time * shakeSpeed) * shakeAmount;
        transform.localPosition = originalPosition + new Vector3(shakeX, shakeY, 0f);
    }
}