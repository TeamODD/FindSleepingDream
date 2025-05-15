using UnityEngine;
using UnityEngine.Video;

public class TriggeredVideoByDestruction : MonoBehaviour
{
    public GameObject targetObject;       // 파괴 여부를 감지할 오브젝트
    public VideoPlayer videoPlayer;       // 비디오 플레이어
    public VideoClip videoIfExists;       // 오브젝트가 존재할 때
    public VideoClip videoIfDestroyed;    // 오브젝트가 파괴되었을 때

    private bool hasPlayed = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasPlayed) return;

        if (other.CompareTag("Player"))
        {
            if (targetObject == null)
            {
                videoPlayer.clip = videoIfDestroyed;
                Debug.Log("🎬 오브젝트 사라짐 → Destroy 영상 재생");
            }
            else
            {
                videoPlayer.clip = videoIfExists;
                Debug.Log("🎬 오브젝트 존재함 → 기본 영상 재생");
            }

            videoPlayer.Play();
            hasPlayed = true;
        }
    }
}
