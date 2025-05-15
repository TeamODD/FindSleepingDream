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
    // 먼저 눌린 상태면 무시 (이전 프레임 잔재)
    yield return new WaitUntil(() => !Input.GetKey(key));

    // 진짜 누르는 순간 기다림
    yield return new WaitUntil(() => Input.GetKeyDown(key));
}


private void SwitchCutsceneSprite(int index)
{
    if (index < 0 || index >= cutsceneSprites.Length) return;

    cutsceneImage.sprite = cutsceneSprites[index];
    showTimer = 0f;
    Debug.Log($"컷씬 전환! Index: {index}");
}


    public void HideCutscene(bool skipTimeResume = false)
{
    cutsceneImage.gameObject.SetActive(false);
    isShowing = false;

    if (!skipTimeResume)
        Time.timeScale = 1f;

    Debug.Log("컷씬 숨김");
}



    void Update()
{
    if (!isShowing) return;

    showTimer += Time.unscaledDeltaTime;

    // 더 이상 여기서 키 입력으로 닫지 않음
    // 키 입력은 시퀀스 내부에서만 처리
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
        Debug.Log($"컷씬 실행! Index: {index}");
    }

    public void HideCutscene()
    {
        cutsceneImage.gameObject.SetActive(false);
        isShowing = false;
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
        // 입력 기다림 (unscaled)
        yield return WaitForKeyDownUnscaled(exitKey);

        SwitchCutsceneSprite(indices[i]);
    }

    yield return WaitForKeyDownUnscaled(exitKey);
    HideCutscene();
    Time.timeScale = 1f;
    currentSequence = null;
}



}
