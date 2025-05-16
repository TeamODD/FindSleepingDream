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

    void Update()
    {

        if (player == null || bgmSource == null || bgmZones == null || bgmZones.Length == 0)
        {
            Debug.LogWarning("🚫 필수 컴포넌트가 없습니다.");
            return;
        }

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
                Debug.Log("🛑 BGM 정지됨");
                bgmSource.Stop();
                bgmSource.clip = null;
            }
            else
            {
                var newClip = bgmZones[matchedIndex].bgmClip;
                if (newClip != null)
                {
                    Debug.Log("▶ BGM 재생: " + newClip.name);
                    bgmSource.Stop();
                    bgmSource.clip = newClip;
                    bgmSource.loop = true;
                    bgmSource.Play();  // 여기가 실제 재생 트리거

                    Debug.Log("✅ Play() 호출됨");

                }
                else
                {
                    Debug.LogWarning("⚠️ bgmZones[" + matchedIndex + "]에 clip이 비어 있음");
                }
            }

            currentZoneIndex = matchedIndex;
        }
        Debug.Log("현재 clip: " + bgmSource.clip?.name);

    }
}
