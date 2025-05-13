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

<<<<<<< HEAD
    // 상태 분기용 enum
    private enum CameraState
    {
        FollowPlayer,
        FixedToBoss
=======
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
>>>>>>> 3efc4f58424ec1202c25322086b71ad0fab1f481
    }

    private CameraState state = CameraState.FollowPlayer;

<<<<<<< HEAD
    [Header("보스 연출 구간")]
    public bool boss1Triggered = false;
    public float boss1TriggerX = 110f;
    public float boss1EndOffset = 20f;
    public GameObject boss1Object; // 보스 오브젝트
    public Transform fixedTargetObject; // 고정 카메라가 바라볼 대상

    // 외부 조회용
=======
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

>>>>>>> 3efc4f58424ec1202c25322086b71ad0fab1f481
    public static int CurrentZone { get; private set; }

    void Start()
    {
        hardTarget = transform.position;
        CurrentZone = 0;
    }

    void Update()
    {
<<<<<<< HEAD
        switch (state)
        {
            case CameraState.FollowPlayer:
                HandleFollowPlayer();
                break;

            case CameraState.FixedToBoss:
                HandleFixedToBoss();
                break;
=======
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
>>>>>>> 3efc4f58424ec1202c25322086b71ad0fab1f481
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

<<<<<<< HEAD
        Vector3 target = hardTarget;
=======
>>>>>>> 3efc4f58424ec1202c25322086b71ad0fab1f481
        float leftBound = hardTarget.x - followMargin;
        float rightBound = hardTarget.x + followMargin;
        float clampedX = Mathf.Clamp(player.position.x, leftBound, rightBound);
        Vector3 target = new Vector3(clampedX, 0f, -5f);

        transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * smoothSpeed);

<<<<<<< HEAD
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
=======
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

>>>>>>> 3efc4f58424ec1202c25322086b71ad0fab1f481
    }

    void HandleFixedToBoss()
    {
<<<<<<< HEAD
        // ✅ 카메라 고정
        if (fixedTargetObject != null)
        {
            Vector3 fixedPos = fixedTargetObject.position;
            transform.position = new Vector3(fixedPos.x, fixedPos.y, transform.position.z);
        }

        // ✅ 연출 종료 조건
=======
        if (player != null && fixedTargetObject != null)
        {
            float midX = (player.position.x + fixedTargetObject.position.x) / 2f;
            float targetX = midX + midPointOffset;
            transform.position = new Vector3(targetX, 0f, -5f);
        }

>>>>>>> 3efc4f58424ec1202c25322086b71ad0fab1f481
        if (player.position.x > boss1TriggerX + boss1EndOffset)
        {
            if (boss1Object != null)
                boss1Object.SetActive(false);

            state = CameraState.FollowPlayer;
<<<<<<< HEAD

            hardTarget = new Vector3(
                transform.position.x,
                transform.position.y,
                transform.position.z
            );
=======
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
            if (boss2Object != null)
                boss2Object.SetActive(false);

            state = CameraState.FollowPlayer;
            returnTarget = new Vector3(player.position.x, 0f, -5f); // 부드럽게 이동할 목표

            hardTarget = returnTarget;
        
>>>>>>> 3efc4f58424ec1202c25322086b71ad0fab1f481
        }
    }
}
