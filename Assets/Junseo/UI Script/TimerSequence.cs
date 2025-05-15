using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TimerSequence : MonoBehaviour
{
    [Header("조건")]
    public Transform player;
    public float triggerX = 10f;
    public float threshold = 0.5f;

    [Header("타이머")]
    public Text timerText;
    public float duration = 15f;

    [Header("보스 이동 제어")]
    public Transform targetObject; // 보스
    // 이동은 BossMove가 처리하므로 X값, speed는 제거해도 OK

    private bool sequenceRunning = false;

    void Update()
    {
        if (!sequenceRunning && Mathf.Abs(player.position.x - triggerX) < threshold)
        {
            StartCoroutine(Sequence());
            sequenceRunning = true;
        }
    }

    IEnumerator Sequence()
    {
        // 1) 타이머 카운트다운
        if (timerText) timerText.gameObject.SetActive(true);
        float t = duration;

        while (t > 0f)
        {
            t -= Time.deltaTime;
            int m = Mathf.FloorToInt(t / 60f);
            int s = Mathf.FloorToInt(t % 60f);
            if (timerText) timerText.text = $"{m:00}:{s:00}";
            yield return null;
        }

        if (timerText) timerText.text = "00:00";
        yield return new WaitForSeconds(0.2f);
        if (timerText) timerText.gameObject.SetActive(false);
if (timerText) timerText.gameObject.SetActive(false);

if (targetObject != null)
{
    BossMove boss = targetObject.GetComponent<BossMove>();
    if (boss != null)
    {
        boss.timer = 0f;
        boss.shouldMove = false;  // 다시 delay 조건 진입
        Debug.Log("🔁 보스 이동 재준비 완료");
    }
}

        }
    }

