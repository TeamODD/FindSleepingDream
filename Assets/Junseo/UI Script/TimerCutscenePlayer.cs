using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TimerCutscenePlayer : MonoBehaviour
{
    public Image cutsceneImage;
    public Sprite[] cutsceneSprites;
    public KeyCode nextKey = KeyCode.C;
    public float unscaledKeyBlockTime = 0.1f;

    private Coroutine currentCutscene = null;
    private bool isPlaying = false;

    private void Update()
    {
        if (!isPlaying) return;

        // Optional: Escape to skip, etc.
    }

    public void PlayCutsceneSequence(params int[] indices)
    {
        if (currentCutscene != null) StopCoroutine(currentCutscene);
        currentCutscene = StartCoroutine(CutsceneRoutine(indices));
    }

    private IEnumerator CutsceneRoutine(int[] indices)
    {
        isPlaying = true;
        Time.timeScale = 0f;

        cutsceneImage.gameObject.SetActive(true);
        cutsceneImage.sprite = cutsceneSprites[indices[0]];
        Debug.Log($"[타이머컷씬] {indices[0]} 시작");

        for (int i = 1; i < indices.Length; i++)
        {
            yield return WaitForKeyDownUnscaled(nextKey);
            cutsceneImage.sprite = cutsceneSprites[indices[i]];
            Debug.Log($"[타이머컷씬] {indices[i]} 전환");
        }

        yield return WaitForKeyDownUnscaled(nextKey);

        // 종료
        cutsceneImage.gameObject.SetActive(false);
        Time.timeScale = 1f;
        isPlaying = false;

        Debug.Log("[타이머컷씬] 종료");
    }

    private IEnumerator WaitForKeyDownUnscaled(KeyCode key)
    {
        yield return new WaitUntil(() => !Input.GetKey(key));      // 키 뗀 다음
        yield return new WaitUntil(() => Input.GetKeyDown(key));  // 다시 누를 때까지 대기
    }
}
