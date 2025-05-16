using UnityEngine;

public class Soundplus : MonoBehaviour
{
    [System.Serializable]
    public class SFXZone
    {
        public float fromX;
        public float toX;
        public AudioClip clip;
        public float volume = 1.0f; // 배경음보다 살짝 큰 소리
        [HideInInspector] public bool hasPlayed = false;
    }

    public Transform player;
    public AudioSource sfxSource;
    public SFXZone[] zones;

    void Update()
    {
        if (player == null || sfxSource == null || zones == null) return;

        float px = player.position.x;

        foreach (var zone in zones)
        {
            if (zone.clip == null) continue;

            if (px >= zone.fromX && px <= zone.toX)
            {
                if (!zone.hasPlayed)
                {
                    sfxSource.PlayOneShot(zone.clip, zone.volume);
                    Debug.Log($" 자동 효과음 재생: {zone.clip.name}");
                    zone.hasPlayed = true;
                }
            }
            else
            {
                // 범위 벗어나면 다시 재생 가능하도록 리셋
                zone.hasPlayed = false;
            }
        }
    }
}
