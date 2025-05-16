using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public enum TransitionType
{
    None,
    Fade
}

[System.Serializable]
public class CutsceneStep
{
    public int cutsceneIndex;
    public float waitTime = 2f;
    public TransitionType transition = TransitionType.None;
    public AudioClip sfx;
    public UnityEvent onTransition;
    public bool isFinalStep = false;
    public bool autoCloseFinal = false;  // ✅ 마지막 컷씬 자동 종료 여부
    public KeyCode finalExitKey = KeyCode.C;
}

public class AutoCutsceneTrigger : MonoBehaviour
{
    public List<CutsceneStep> steps = new List<CutsceneStep>();
    public float fadeDuration = 1f;
    public AudioSource audioSource;

    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered || !other.CompareTag("Player")) return;

        triggered = true;
        StartCoroutine(PlayCutsceneSequence());
    }

    private IEnumerator PlayCutsceneSequence()
    {
        var manager = FindFirstObjectByType<CutsceneManager>();
        if (manager == null)
        {
            Debug.LogWarning("CutsceneManager 없음!");
            yield break;
        }

        Image image = manager.cutsceneImage;

        foreach (var step in steps)
        {
            step.onTransition?.Invoke();

            if (step.sfx != null && audioSource != null)
                audioSource.PlayOneShot(step.sfx);

            if (step.transition == TransitionType.Fade)
                yield return FadeOut(image, fadeDuration);

            manager.ShowCutscene(step.cutsceneIndex);

            if (step.transition == TransitionType.Fade)
                yield return FadeIn(image, fadeDuration);

            if (step.isFinalStep)
            {
                if (step.autoCloseFinal)
                {
                    yield return new WaitForSecondsRealtime(step.waitTime);
                    manager.HideCutscene();
                }
                else
                {
                    yield return new WaitUntil(() => Input.GetKeyDown(step.finalExitKey));
                    manager.HideCutscene();
                }
            }
            else
            {
                yield return new WaitForSecondsRealtime(step.waitTime);
                manager.HideCutscene(skipTimeResume: true);
            }
        }

        Time.timeScale = 1f;
         yield return new WaitForSecondsRealtime(5f);
    Destroy(gameObject);

    }

    private IEnumerator FadeOut(Image img, float duration)
    {
        Color color = img.color;
        float time = 0f;
        while (time < duration)
        {
            time += Time.unscaledDeltaTime;
            color.a = Mathf.Lerp(1f, 0f, time / duration);
            img.color = color;
            yield return null;
        }
        color.a = 0f;
        img.color = color;
    }

    private IEnumerator FadeIn(Image img, float duration)
    {
        Color color = img.color;
        float time = 0f;
        while (time < duration)
        {
            time += Time.unscaledDeltaTime;
            color.a = Mathf.Lerp(0f, 1f, time / duration);
            img.color = color;
            yield return null;
        }
        color.a = 1f;
        img.color = color;
    }
}
