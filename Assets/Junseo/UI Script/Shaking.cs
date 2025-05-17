using UnityEngine;

public class Shaking : MonoBehaviour
{
    public float ShakeAmount=0.05f;
    public float ShakeSpeed=10f;
    private Vector3 baseposition;

    void Start()
    {
       baseposition=transform.localPosition;
    }

    void Update()
    {
        float shakeX = Mathf.Sin(Time.time * ShakeSpeed) * ShakeAmount;
        float shakeY = Mathf.Cos(Time.time * ShakeSpeed) * ShakeAmount;
        transform.localPosition = baseposition + new Vector3(shakeX, shakeY , 0f);
    }


}
