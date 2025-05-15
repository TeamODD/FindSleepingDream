using UnityEngine;

public class MoveOnTimerEnd : MonoBehaviour
{
    public Timer timerScript;
    public Transform targetObject;
    public float targetXPosition = 5f;
    public float moveSpeed = 2f;

    private bool shouldMove = false;
    private Vector3 targetPosition;

    void Update()
    {
        // ✅ 타이머가 막 시작됐을 때 targetObject의 Y/Z 기준으로 목표 좌표 계산
        if (timerScript != null && timerScript.TimerJustStarted() && targetObject != null)
        {
            Vector3 start = targetObject.position;
            targetPosition = new Vector3(targetXPosition, start.y, start.z);
            Debug.Log($"🎯 이동 목표 좌표 저장: {targetPosition}");
        }

        // ✅ 타이머 끝나면 이동 시작
        if (!shouldMove && timerScript != null && timerScript.IsTimerFinished())
        {
            shouldMove = true;
            Debug.Log("🟢 이동 시작!");
        }

        // ✅ 이동 처리
        if (shouldMove && targetObject != null)
        {
            targetObject.position = Vector3.MoveTowards(targetObject.position, targetPosition, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(targetObject.position, targetPosition) < 0.01f)
            {
                shouldMove = false;
                Debug.Log("✅ 이동 완료");
            }
        }
    }
}
