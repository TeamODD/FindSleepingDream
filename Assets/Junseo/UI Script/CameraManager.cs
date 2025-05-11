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

    // 상태 분기용 enum
    private enum CameraState
    {
        FollowPlayer,
        FixedToBoss
    }

    private CameraState state = CameraState.FollowPlayer;

    [Header("보스 연출 구간")]
    public bool boss1Triggered = false;
    public float boss1TriggerX = 110f;
    public float boss1EndOffset = 20f;
    public GameObject boss1Object; // 보스 오브젝트
    public Transform fixedTargetObject; // 고정 카메라가 바라볼 대상

    // 외부 조회용
    public static int CurrentZone { get; private set; }

    void Start()
    {
        hardTarget = transform.position;
        CurrentZone = 0;
    }

    void Update()
    {
        switch (state)
        {
            case CameraState.FollowPlayer:
                HandleFollowPlayer();
                break;

            case CameraState.FixedToBoss:
                HandleFixedToBoss();
                break;
        }
    }

    void HandleFollowPlayer()
    {
        int zone = -1;
        for (int i = triggerXPositions.Length - 1; i >= 0; i--)
        {
            if (player.position.x > triggerXPositions[i])
            {
                zone = i;
                break;
            }
        }

        if (zone == -1) zone = 0;

        if (zone != currentZone)
        {
            currentZone = zone;
            CurrentZone = zone;

            hardTarget = new Vector3(
                cameraPositions[zone].x,
                cameraPositions[zone].y,
                transform.position.z
            );

            transform.position = hardTarget;
            return;
        }

        Vector3 target = hardTarget;
        float leftBound = hardTarget.x - followMargin;
        float rightBound = hardTarget.x + followMargin;
        float clampedX = Mathf.Clamp(player.position.x, leftBound, rightBound);
        target.x = clampedX;

        transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * smoothSpeed);

        // ✅ 보스 트리거
        if (!boss1Triggered && player.position.x > boss1TriggerX)
        {
            boss1Triggered = true;

            if (boss1Object != null)
                boss1Object.SetActive(true);

            if (fixedTargetObject != null)
            {
                transform.position = new Vector3(
                    fixedTargetObject.position.x,
                    fixedTargetObject.position.y,
                    transform.position.z
                );
            }

            state = CameraState.FixedToBoss;
        }
    }

    void HandleFixedToBoss()
    {
        // ✅ 카메라 고정
        if (fixedTargetObject != null)
        {
            Vector3 fixedPos = fixedTargetObject.position;
            transform.position = new Vector3(fixedPos.x, fixedPos.y, transform.position.z);
        }

        // ✅ 연출 종료 조건
        if (player.position.x > boss1TriggerX + boss1EndOffset)
        {
            if (boss1Object != null)
                boss1Object.SetActive(false);

            state = CameraState.FollowPlayer;

            hardTarget = new Vector3(
                transform.position.x,
                transform.position.y,
                transform.position.z
            );
        }
    }
}
