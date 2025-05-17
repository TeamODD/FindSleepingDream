using UnityEngine;
using System.Collections; // ← Coroutine 사용을 위해 추가

public class MusicTest : MonoBehaviour
{
    [System.Serializable]
    public class BGMZone
    {
        public float fromX;
        public float toX;
        public AudioClip clip;
        public float delay = 0f; //  인스펙터에서 조절 가능한 딜레이 시간 (초)
    }

    public Transform player;            // 플레이어 오브젝트 연결
    public AudioSource bgmSource;       // 오디오 소스 연결
    public BGMZone[] zones;             // 구간 설정

    private int currentZoneIndex = -1;
    private AudioClip currentClip = null;

    private Coroutine delayCoroutine = null;
    private int pendingIndex = -1;  //  현재 재생 대기중인 코루틴 추적

    void Update()
    {
        if (player == null || bgmSource == null || zones == null) return;

        float px = player.position.x;
        int matchedIndex = -1;

        for (int i = 0; i < zones.Length; i++)
        {
            var zone = zones[i];
            if (zone.clip == null) continue;

            // 비어있는 Clip은 무시

            if (px >= zone.fromX && px <= zone.toX)
            {
                matchedIndex = i;
                break;
            }
        }

        if (matchedIndex != -1 && matchedIndex != currentZoneIndex)
        {
            SwitchBGM(matchedIndex);
        }
    }

    void SwitchBGM(int index)
    {
        AudioClip newClip = zones[index].clip;
        float delay = zones[index].delay; //  해당 구간의 딜레이 시간

        if (newClip == currentClip) return;

        //  이전에 대기 중인 재생이 있으면 취소
      
       
        if (delay > 0f)
        {
            pendingIndex = index;
            if (delayCoroutine == null)
            {
                delayCoroutine = StartCoroutine(PlayAfterDelay(newClip, index, delay));
                Debug.Log("재새여부");
            }
        }
        else
        {
            PlayBGM(newClip, index);
        }
    }

    IEnumerator PlayAfterDelay(AudioClip clip, int index, float delay)
    {
        yield return new WaitForSeconds(delay);
        PlayBGM(clip, index);
        delayCoroutine = null;
        pendingIndex = -1;
        currentZoneIndex = index;
        Debug.Log("재생 여부 22");
    }

    void PlayBGM(AudioClip clip, int index)
    {
        bgmSource.Stop();
        bgmSource.clip = clip;
        bgmSource.loop = true;
        bgmSource.Play();

        currentClip = clip;
        currentZoneIndex = index;
        Debug.Log($" BGM 전환: {clip.name}");
    }
}
