using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [Header("�ʼ� ����")]
    public Transform player;
    public Vector3[] cameraPositions;
    public float[] triggerXPositions;
    public float smoothSpeed = 3f; // �ε巴�� �̵��� �ӵ�

    private int currentZone = -1;
    private Vector3 targetPosition;

    void Start()
    {
        targetPosition = transform.position;
    }

    void Update()
    {
        int zone = -1;

        // ���������� �� ���� �ش� zone ����
        for (int i = triggerXPositions.Length - 1; i >= 0; i--)
        {
            if (player.position.x > triggerXPositions[i])
            {
                zone = i;
                break;
            }
        }

        // ���� �ƹ� ���ǵ� �� �����ϸ� zone = -1 �� zone 0���� ����
        if (zone == -1) zone = 0;

        if (zone != currentZone)
        {
            currentZone = zone;
            targetPosition = new Vector3(
                cameraPositions[zone].x,
                cameraPositions[zone].y,
                transform.position.z
            );
        }

        transform.position = targetPosition;

    }

}
