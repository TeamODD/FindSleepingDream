using UnityEngine;

public class Soundplus : MonoBehaviour
{
    [System.Serializable]
    public class SFXZone
    {
        public float fromX;
        public float toX;
        public AudioClip clip;
        public float volume = 1.0f;
        //  [HideInInspector] public bool hasPlayed = false; // ← 제거됨
    }

    public Transform player;
    public AudioSource sfxSource;
    public SFXZone[] zones;

    // 추가됨: 현재 재생 중인 구간 인덱스
    private int currentZoneIndex = -1;

    void Update()
    {
        if (player == null || sfxSource == null || zones == null) return;

        float px = player.position.x;
        int matchedIndex = -1;

        for (int i = 0; i < zones.Length; i++)
        {
            var zone = zones[i];
            if (zone.clip == null) continue;

            if (px >= zone.fromX && px <= zone.toX)
            {
                matchedIndex = i;
                break;
            }
        }

        //  조건 변경: 이전과 다른 구간일 경우에만 재생
        if (matchedIndex != -1 && matchedIndex != currentZoneIndex)
        {
            //  이전 소리 끊기
            sfxSource.Stop();

            //  PlayOneShot → Play 방식으로 변경
            sfxSource.clip = zones[matchedIndex].clip;
            sfxSource.volume = zones[matchedIndex].volume;
            sfxSource.Play();

            //  현재 구간 기억
            currentZoneIndex = matchedIndex;

            Debug.Log($" 자동 효과음 재생 (단일): {zones[matchedIndex].clip.name}");
        }

        // 구간 밖으로 나갔으면 소리 멈춤
        if (matchedIndex == -1 && currentZoneIndex != -1)
        {
            sfxSource.Stop();
            currentZoneIndex = -1;
        }
    }
}
