using UnityEngine;
using UnityEngine.UI;

public class CutsceneManager : MonoBehaviour
{
    public Image cutsceneImage;
    public Sprite[] cutsceneSprites;
    public KeyCode exitKey = KeyCode.Space;

    private bool isShowing = false;
    private float showTimer = 0f;
    private float keyBlockTime = 0.1f; // 키 입력 무시 시간 (초)

    void Update()
    {
        if (isShowing)
        {
            showTimer += Time.unscaledDeltaTime;

            if (Input.GetKeyDown(exitKey) && showTimer > keyBlockTime)
            {
                HideCutscene();
            }
        }
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
        showTimer = 0f; // 시간 초기화

        Time.timeScale = 0f;
        Debug.Log("컷씬 실행! Index: " + index);
    }

    public void HideCutscene()
    {
        cutsceneImage.gameObject.SetActive(false);
        isShowing = false;
        Time.timeScale = 1f;
        Debug.Log("컷씬 숨김");
        
    }
}

