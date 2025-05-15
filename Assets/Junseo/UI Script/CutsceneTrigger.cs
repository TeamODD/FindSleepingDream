using UnityEngine;
using System.Collections;

public class CutsceneTriggerSequence : MonoBehaviour
{
    public int[] cutsceneIndices;           // 🔥 Inspector에서 설정
    private bool triggered = false;


    public AudioSource audiosource; // 오디오추가



    private IEnumerator DelayedAudioPlay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay); // ⏳ 게임 정지 무시하고 시간 지연
        AudioPlay(); // 🎵 오디오 재생
    }

    void AudioPlay()
    {
        if (!audiosource.isPlaying)
            audiosource.Play();

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered || !other.CompareTag("Player")) return;
        triggered = true;

        var manager = FindFirstObjectByType<CutsceneManager>();
        if (manager != null)
        {
            manager.ShowCutsceneSequence(cutsceneIndices);
        }

        StartCoroutine(DelayedAudioPlay(2f));
    }
}
