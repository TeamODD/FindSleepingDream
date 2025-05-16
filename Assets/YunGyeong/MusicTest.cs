using UnityEngine;

public class MusicTest : MonoBehaviour
{
    [System.Serializable]
    public class BGMZone
    {
        public float fromX;
        public float toX;
        public AudioClip clip;
    }

    public Transform player;            // 플레이어 오브젝트 연결
    public AudioSource bgmSource;       // 오디오 소스 연결
    public BGMZone[] zones;             // 구간 설정

    private int currentZoneIndex = -1;
    private AudioClip currentClip = null;

    void Update()
    {
        if (player == null || bgmSource == null || zones == null) return;

        float px = player.position.x;
        int matchedIndex = -1;

        for (int i = 0; i < zones.Length; i++)
        {
            var zone = zones[i];

            // 비어있는 Clip은 무시
            if (zone.clip == null) continue;

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

        if (newClip == currentClip) return;

        bgmSource.Stop();
        bgmSource.clip = newClip;
        bgmSource.loop = true;
        bgmSource.Play();

        currentClip = newClip;
        currentZoneIndex = index;
        Debug.Log($" BGM 전환: {newClip.name}");
    }
}
