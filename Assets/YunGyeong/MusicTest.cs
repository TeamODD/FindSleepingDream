using UnityEngine;
using System.Collections; // �� Coroutine ����� ���� �߰�

public class MusicTest : MonoBehaviour
{
    [System.Serializable]
    public class BGMZone
    {
        public float fromX;
        public float toX;
        public AudioClip clip;
        public float delay = 0f; //  �ν����Ϳ��� ���� ������ ������ �ð� (��)
    }

    public Transform player;            // �÷��̾� ������Ʈ ����
    public AudioSource bgmSource;       // ����� �ҽ� ����
    public BGMZone[] zones;             // ���� ����

    private int currentZoneIndex = -1;
    private AudioClip currentClip = null;

    private Coroutine delayCoroutine = null;
    private int pendingIndex = -1;  //  ���� ��� ������� �ڷ�ƾ ����

    void Update()
    {
        if (player == null || bgmSource == null || zones == null) return;

        float px = player.position.x;
        int matchedIndex = -1;

        for (int i = 0; i < zones.Length; i++)
        {
            var zone = zones[i];
            if (zone.clip == null) continue;

            // ����ִ� Clip�� ����

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
        float delay = zones[index].delay; //  �ش� ������ ������ �ð�

        if (newClip == currentClip) return;

        //  ������ ��� ���� ����� ������ ���
      
       
        if (delay > 0f)
        {
            pendingIndex = index;
            if (delayCoroutine == null)
            {
                delayCoroutine = StartCoroutine(PlayAfterDelay(newClip, index, delay));
                Debug.Log("�������");
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
        Debug.Log("��� ���� 22");
    }

    void PlayBGM(AudioClip clip, int index)
    {
        bgmSource.Stop();
        bgmSource.clip = clip;
        bgmSource.loop = true;
        bgmSource.Play();

        currentClip = clip;
        currentZoneIndex = index;
        Debug.Log($" BGM ��ȯ: {clip.name}");
    }
}
