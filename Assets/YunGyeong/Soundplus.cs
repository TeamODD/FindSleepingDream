using UnityEngine;

public class Soundplus : MonoBehaviour
{
    [System.Serializable]
    public class SFXZone
    {
        public float fromX;
        public float toX;
        public AudioClip clip;
        public float volume = 1.0f; // ��������� ��¦ ū �Ҹ�
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
                    Debug.Log($" �ڵ� ȿ���� ���: {zone.clip.name}");
                    zone.hasPlayed = true;
                }
            }
            else
            {
                // ���� ����� �ٽ� ��� �����ϵ��� ����
                zone.hasPlayed = false;
            }
        }
    }
}
