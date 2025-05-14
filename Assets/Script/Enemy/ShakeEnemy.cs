using UnityEngine;

public class ShakeEnemy : MonoBehaviour
{
    public float shakeAmount = 0.05f;
    public float shakeSpeed = 10f;

    private float originalY;
    private float originalZ;

    void Start()
    {
        originalY = transform.localPosition.y;
        originalZ = transform.localPosition.z;
    }

    void Update()
    {
        float shakeY = Mathf.Cos(Time.time * shakeSpeed) * shakeAmount;

        // ✅ X는 건드리지 않음!
        transform.localPosition = new Vector3(
            transform.localPosition.x,                 // X: BossMove가 컨트롤
            originalY + shakeY,                        // Y: 흔들림
            originalZ                                   // Z: 그대로
        );
    }
}
