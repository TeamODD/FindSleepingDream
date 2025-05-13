using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Timer : MonoBehaviour
{
    public Transform player;
    public Vector3 targetPosition = new Vector3(10, 0, 0);
    public float positionThreshold = 0.5f;

    public Text timerText;  // 연결 필수

    private bool timerStarted = false;
    private float timer = 0f;
    public float duration = 15f;  // 15초부터 시작

    void Update()
    {
        // ✅ X축 조건 확인
        if (!timerStarted && Mathf.Abs(player.position.x - targetPosition.x) < positionThreshold)
        {
            timerStarted = true;
            timer = duration;

            if (timerText != null)
                timerText.gameObject.SetActive(true);

            Debug.Log("타이머 시작!");
        }

        // ✅ 타이머 작동 중
        if (timerStarted)
        {
            timer -= Time.deltaTime;

            if (timer > 0f)
            {
                UpdateTimerDisplay(timer);
            }
            else
            {
                timer = 0f;
                UpdateTimerDisplay(timer);
                timerStarted = false;
                StartCoroutine(HideTextAfterDelay(0.2f));
            }
        }
    }

    void UpdateTimerDisplay(float timeRemaining)
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60f);
        int seconds = Mathf.FloorToInt(timeRemaining % 60f);
        if (timerText != null)
            timerText.text = $"{minutes:00}:{seconds:00}";
    }

    IEnumerator HideTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (timerText != null)
            timerText.gameObject.SetActive(false);
        Debug.Log("타이머 숨김");
    }
}
