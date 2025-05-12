using UnityEngine;

public class ShakeDoor : MonoBehaviour
{
    public float shakeAmount = 0.05f;
    public float shakeSpeed = 10f;
    private Vector3 basePosition;

    void Start()
    {
        basePosition = transform.position; // ó�� ��ġ ���� <- �̰� �ؾ� ����� ��鸲
    }

    void LateUpdate()
    {
        float shakeX = Mathf.Sin(Time.time * shakeSpeed) * shakeAmount;
        //float shakeY = Mathf.Cos(Time.time * shakeSpeed) * shakeAmount;

        transform.position = basePosition + new Vector3(shakeX, 0f, 0f);
    }
}