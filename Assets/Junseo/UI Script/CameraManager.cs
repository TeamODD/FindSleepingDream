using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [Header("필수 설정")]
    public Transform player;
    public Vector3[] cameraPositions;
    public float[] triggerXPositions;
    public float smoothSpeed = 3f; // 부드럽게 이동할 속도

    private int currentZone = -1;
    private Vector3 targetPosition;

    void Start()
    {
        targetPosition = transform.position;
    }

    void Update()
    {
        int zone = -1;

        // 오른쪽으로 갈 때는 해당 zone 결정
        for (int i = triggerXPositions.Length - 1; i >= 0; i--)
        {
            if (player.position.x > triggerXPositions[i])
            {
                zone = i;
                break;
            }
        }

        // 만약 아무 조건도 안 만족하면 zone = -1 → zone 0으로 간주
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
