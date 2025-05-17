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
        //  [HideInInspector] public bool hasPlayed = false; // �� ���ŵ�
    }

    public Transform player;
    public AudioSource sfxSource;
    public SFXZone[] zones;

    // �߰���: ���� ��� ���� ���� �ε���
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

        //  ���� ����: ������ �ٸ� ������ ��쿡�� ���
        if (matchedIndex != -1 && matchedIndex != currentZoneIndex)
        {
            //  ���� �Ҹ� ����
            sfxSource.Stop();

            //  PlayOneShot �� Play ������� ����
            sfxSource.clip = zones[matchedIndex].clip;
            sfxSource.volume = zones[matchedIndex].volume;
            sfxSource.Play();

            //  ���� ���� ���
            currentZoneIndex = matchedIndex;

            Debug.Log($" �ڵ� ȿ���� ��� (����): {zones[matchedIndex].clip.name}");
        }

        // ���� ������ �������� �Ҹ� ����
        if (matchedIndex == -1 && currentZoneIndex != -1)
        {
            sfxSource.Stop();
            currentZoneIndex = -1;
        }
    }
}
