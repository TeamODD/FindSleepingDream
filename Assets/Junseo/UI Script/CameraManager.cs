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

    private bool isReturningFromBoss = false;
    private bool isEnteringBoss = false;
    private Vector3 enterTarget;
    private Vector3 returnTarget;

    private CameraState nextStateAfterEnter; // 보스 진입 시 어떤 상태로 바뀔지 저장



    private enum CameraState
    {
        FollowPlayer,
        FixedToBoss,
        FixedToBoss2
    }

    private CameraState state = CameraState.FollowPlayer;

    [Header("보스 1 연출 구간")]
    public bool boss1Triggered = false;
    public float boss1TriggerX = 110f;
    public float boss1EndOffset = 20f;
    public GameObject boss1Object;
    public Transform fixedTargetObject;

    [Header("보스 2 연출 구간")]
    public bool boss2Triggered = false;
    public float boss2TriggerX = 210f;
    public float boss2EndOffset = 20f;
    public GameObject boss2Object;
    public Transform fixedTargetObject2;

    [Header("보스 연출 카메라 옵션")]
    public float midPointOffset = 0f;

    public static int CurrentZone { get; private set; }

    void Start()
    {
        hardTarget = transform.position;
        CurrentZone = 0;
    }

    void Update()
    {
        if (isEnteringBoss)
{
    transform.position = Vector3.Lerp(transform.position, enterTarget, Time.deltaTime * smoothSpeed);

    if (Vector3.Distance(transform.position, enterTarget) < 0.05f)
    {
        transform.position = enterTarget;
        isEnteringBoss = false;

        // ✅ 실제 전환할 상태로 바꾸기
        state = nextStateAfterEnter;
    }
    return;
}


        if (isReturningFromBoss)
        {
        transform.position = Vector3.Lerp(transform.position, returnTarget, Time.deltaTime * smoothSpeed);

        if (Vector3.Distance(transform.position, returnTarget) < 0.05f)
        {
            transform.position = returnTarget;
            isReturningFromBoss = false;
            hardTarget = returnTarget; // zone 기준도 갱신
        }

        return; // 다른 상태 처리 중단
        }

        switch (state)
        {
            case CameraState.FollowPlayer:
                HandleFollowPlayer();
                break;
            case CameraState.FixedToBoss:
                HandleFixedToBoss();
                break;
            case CameraState.FixedToBoss2:
                HandleFixedToBoss2();
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
                0f,
                -5f
            );

            transform.position = hardTarget;
            return;
        }

        float leftBound = hardTarget.x - followMargin;
        float rightBound = hardTarget.x + followMargin;
        float clampedX = Mathf.Clamp(player.position.x, leftBound, rightBound);
        Vector3 target = new Vector3(clampedX, 0f, -5f);

        transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * smoothSpeed);

        // 보스 1 트리거
        if (!boss1Triggered && player.position.x > boss1TriggerX)
{
    boss1Triggered = true;

    if (boss1Object != null)
        boss1Object.SetActive(true);

    isEnteringBoss = true;
    float midX = (player.position.x + fixedTargetObject.position.x) / 2f;
    enterTarget = new Vector3(midX + midPointOffset, 0f, -5f);

    nextStateAfterEnter = CameraState.FixedToBoss; // ✅ 상태 저장
}


        // 보스 2 트리거
        if (!boss2Triggered && player.position.x > boss2TriggerX)
{
    boss2Triggered = true;

    if (boss2Object != null)
        boss2Object.SetActive(true);

    isEnteringBoss = true;
    float midX = (player.position.x + fixedTargetObject2.position.x) / 2f;
    enterTarget = new Vector3(midX + midPointOffset, 0f, -5f);

    nextStateAfterEnter = CameraState.FixedToBoss2; // ✅ 상태 저장
}

    }

    void HandleFixedToBoss()
    {
        if (player != null && fixedTargetObject != null)
        {
            float midX = (player.position.x + fixedTargetObject.position.x) / 2f;
            float targetX = midX + midPointOffset;
            transform.position = new Vector3(targetX, 0f, -5f);
        }

        if (player.position.x > boss1TriggerX + boss1EndOffset)
        {
            //if (boss1Object != null)
            //    boss1Object.SetActive(false);

            state = CameraState.FollowPlayer;
            isReturningFromBoss = true;
            returnTarget = new Vector3(player.position.x, 0f, -5f); // 부드럽게 이동할 목표

            hardTarget = returnTarget;
            
        }
    }

    void HandleFixedToBoss2()
    {
        if (player != null && fixedTargetObject2 != null)
        {
            float midX = (player.position.x + fixedTargetObject2.position.x) / 2f;
            float targetX = midX + midPointOffset;
            transform.position = new Vector3(targetX, 0f, -5f);
        }

        if (player.position.x > boss2TriggerX + boss2EndOffset)
        {
            state = CameraState.FollowPlayer;
            returnTarget = new Vector3(player.position.x, 0f, -5f);
            hardTarget = returnTarget;
            isReturningFromBoss = true;
        }
    }
}
