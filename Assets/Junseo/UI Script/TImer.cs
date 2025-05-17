using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Timer : MonoBehaviour
{
    [Header("플레이어 & 타이머 범위")]
    public Transform player;
    public float startX = 5f;
    public float endX = 15f;
    public float duration = 15f;

    [Header("UI")]
    public Text timerText;

    [Header("컷씬 & 이벤트")]
    public int cutsceneIndexToPlay = 0;
    public Vector3 playerTeleportPosition;

    private float timer = 0f;
    private bool timerStarted = false;
    private bool timerJustStarted = false;
    private bool waitingForCutsceneToEnd = false;


    private CutsceneManager cutsceneManager;

    void Start()
    {
        cutsceneManager = FindFirstObjectByType<CutsceneManager>();
    }

    void Update()
    {
        timerJustStarted = false;

        bool isInRange = IsPlayerInRange();

        if (!timerStarted && isInRange)
        {
            timerStarted = true;
            timer = duration;
            timerJustStarted = true;
            ShowTimerTextIfInRange();
            Debug.Log("타이머 시작!");
        }

        if (timerStarted && !isInRange)
        {
            timerStarted = false;
            timer = 0f;
            HideTimerText();
            Debug.Log("⛔ 범위 벗어남 → 타이머 중단");
            return;
        }

        if (timerStarted)
        {
            timer -= Time.deltaTime;

            if (timer > 0f)
            {
                UpdateTimerDisplay(timer);
                ShowTimerTextIfInRange();
            }
            else
            {
                timer = 0f;
                UpdateTimerDisplay(timer);
                timerStarted = false;
                StartCoroutine(HandleTimerEndEvent());
            }
        }
    }

    private bool IsPlayerInRange()
    {
        float px = player.position.x;
        return px >= startX && px <= endX;
    }

    void UpdateTimerDisplay(float timeRemaining)
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60f);
        int seconds = Mathf.FloorToInt(timeRemaining % 60f);
        if (timerText != null)
            timerText.text = $"{minutes:00}:{seconds:00}";
    }

    void ShowTimerTextIfInRange()
    {
        if (timerText != null && IsPlayerInRange())
            timerText.gameObject.SetActive(true);
    }

    void HideTimerText()
    {
        if (timerText != null)
            timerText.gameObject.SetActive(false);
    }

    private IEnumerator HandleTimerEndEvent()
{
    Debug.Log("타이머 종료 → 컷씬 실행");

    if (player != null)
        player.position = playerTeleportPosition;

    // 컷씬 항상 실행
    if (cutsceneManager != null)
    {
        cutsceneManager.ShowCutsceneSequence(cutsceneIndexToPlay);
        waitingForCutsceneToEnd = true;
    }

    yield return new WaitForSeconds(1f);

    if (IsPlayerInRange())
        ShowTimerTextIfInRange();
    else
        HideTimerText();
}


    // ✅ 컷씬 종료 시 호출됨
    public void OnCutsceneEnded(int index)
    {
        if (waitingForCutsceneToEnd && index == cutsceneIndexToPlay)
        {
            waitingForCutsceneToEnd = false;

            if (IsPlayerInRange())
            {
                timer = duration;
                timerStarted = true;
                ShowTimerTextIfInRange();
                Debug.Log("컷씬 종료 → 타이머 재시작");
            }
            else
            {
                HideTimerText();
                Debug.Log("컷씬 종료 → 범위 밖이라 대기");
            }
        }
    }

    public bool IsTimerFinished() => !timerStarted && timer <= 0f;
    public bool TimerJustStarted() => timerJustStarted;
}
