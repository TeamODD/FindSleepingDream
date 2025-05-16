using UnityEngine;

public class BGMTrigger : MonoBehaviour
{
    [System.Serializable]
    public class BGMZone
    {
        public float fromX;
        public float toX;
        public AudioClip bgmClip;
    }

    public Transform player;
    public AudioSource bgmSource;
    public BGMZone[] bgmZones;

    private int currentZoneIndex = -1;
    private AudioClip currentClip = null;

    void Update()
    {
        if (player == null || bgmZones == null || bgmZones.Length == 0) return;

        float px = player.position.x;
        int matchedIndex = -1;

        for (int i = 0; i < bgmZones.Length; i++)
        {
            if (px >= bgmZones[i].fromX && px <= bgmZones[i].toX)
            {
                matchedIndex = i;
                break;
            }
        }

        if (matchedIndex != currentZoneIndex)
        {
            if (matchedIndex == -1)
            {
                // 영역 벗어나면 음악 끄기
                bgmSource.Stop();
                currentClip = null;
                currentZoneIndex = -1;
            }
            else
            {
                PlayZoneBGM(matchedIndex);
            }
        }
    }

    void PlayZoneBGM(int index)
    {
        AudioClip newClip = bgmZones[index].bgmClip;
        if (newClip == null || newClip == currentClip) return;

        bgmSource.Stop(); // 혹시 이전 음악이 남아 있을 수도 있으므로 stop 먼저
        bgmSource.PlayOneShot(newClip);

        currentClip = newClip;
        currentZoneIndex = index;
        Debug.Log("▶ 구간 BGM 재생: " + newClip.name);
    }
}
