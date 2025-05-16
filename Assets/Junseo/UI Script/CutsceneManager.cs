using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CutsceneManager : MonoBehaviour
{
    public Image cutsceneImage;
    public Sprite[] cutsceneSprites;
    public KeyCode exitKey = KeyCode.C;

    private bool isShowing = false;
    private float showTimer = 0f;
    private const float keyBlockTime = 0.1f;

    private Coroutine currentSequence = null;

    private IEnumerator WaitForKeyDownUnscaled(KeyCode key)
    {
        yield return new WaitUntil(() => !Input.GetKey(key));
        yield return new WaitUntil(() => Input.GetKeyDown(key));
    }

    private void SwitchCutsceneSprite(int index)
    {
        if (index < 0 || index >= cutsceneSprites.Length) return;
        cutsceneImage.sprite = cutsceneSprites[index];
        showTimer = 0f;
        Debug.Log($"컷씬 전환! Index: {index}");
    }

    public void ShowCutscene(int index)
    {
        if (index < 0 || index >= cutsceneSprites.Length)
        {
            Debug.LogWarning("컷씬 인덱스 범위 초과");
            return;
        }

        cutsceneImage.sprite = cutsceneSprites[index];
        cutsceneImage.gameObject.SetActive(true);
        isShowing = true;
        showTimer = 0f;

        Time.timeScale = 0f;
    }

    public void HideCutscene(bool skipTimeResume = false)
    {
        cutsceneImage.gameObject.SetActive(false);
        isShowing = false;

        if (!skipTimeResume)
            Time.timeScale = 1f;

        Debug.Log("컷씬 숨김");
    }

    public void ShowCutsceneSequence(params int[] indices)
    {
        if (currentSequence != null) StopCoroutine(currentSequence);
        currentSequence = StartCoroutine(ShowSequenceCoroutine(indices));
    }

    private IEnumerator ShowSequenceCoroutine(int[] indices)
    {
        ShowCutscene(indices[0]);

        for (int i = 1; i < indices.Length; i++)
        {
            yield return WaitForKeyDownUnscaled(exitKey);
            SwitchCutsceneSprite(indices[i]);
        }

        yield return WaitForKeyDownUnscaled(exitKey);

        // 컷씬 끝났다고 알림
        NotifyCutsceneEnded(indices[indices.Length - 1]);

        HideCutscene();
        Time.timeScale = 1f;
        currentSequence = null;
    }

    private void NotifyCutsceneEnded(int index)
    {
        var allActions = FindObjectsOfType<AfterCutscene>();
        foreach (var action in allActions)
        {
            action.OnCutsceneEnded(index);
        }

        var allResetters = FindObjectsOfType<GameOverResetManager>();
        foreach (var resetter in allResetters)
        {
            resetter.OnCutsceneEnded(index);
        }
    }

    private void Update()
    {
        if (!isShowing) return;
        showTimer += Time.unscaledDeltaTime;
    }
}
