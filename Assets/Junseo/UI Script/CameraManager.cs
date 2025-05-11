using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Transform player;
    public Vector3[] cameraPositions;
    public float[] triggerXPositions;
    public float followMargin = 4f;
    public float smoothSpeed = 5f;

    private int currentZone = -1;
    private Vector3 hardTarget;

    // ✅ 다른 스크립트에서 접근 가능하도록
    public static int CurrentZone { get; private set; }

    void Start()
    {
        hardTarget = transform.position;
        CurrentZone = 0;
    }

    void Update()
    {
        int zone = -1;
        Debug.Log("현재 존: " + CameraManager.CurrentZone); 

        // 현재 zone 결정
        for (int i = triggerXPositions.Length - 1; i >= 0; i--)
        {
            if (player.position.x > triggerXPositions[i])
            {
                zone = i;
                break;
            }
        }

        if (zone == -1) zone = 0;

        // zone 바뀌면 하드 이동 + 현재 zone 업데이트
        if (zone != currentZone)
        {
            currentZone = zone;
            CurrentZone = zone;

            hardTarget = new Vector3(
                cameraPositions[zone].x,
                cameraPositions[zone].y,
                transform.position.z
            );

            transform.position = hardTarget; // 즉시 이동
            return; // 한 프레임 쉬고 다음 프레임부터 따라가기
        }

        // 현재 zone 내부에서는 followMargin 안에서 부드럽게 따라감
        Vector3 target = hardTarget;
        float leftBound = hardTarget.x - followMargin;
        float rightBound = hardTarget.x + followMargin;

        float clampedX = Mathf.Clamp(player.position.x, leftBound, rightBound);
        target.x = clampedX;

        transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * smoothSpeed);
    }
}
