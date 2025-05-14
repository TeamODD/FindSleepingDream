using UnityEngine;
using UnityEngine.Video;

public class VideoManager : MonoBehaviour
{
    public GameObject targetObject;       // 상태 확인할 오브젝트
    public VideoPlayer videoPlayer;       // 비디오 플레이어
    public VideoClip videoIfTrue;         // targetObject가 활성화일 때
    public VideoClip videoIfFalse;        // targetObject가 비활성화일 때

    private bool hasPlayed = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasPlayed) return;

        if (other.CompareTag("Player")) // 플레이어 태그로 제한
        {
            if (targetObject.activeSelf)
            {
                videoPlayer.clip = videoIfTrue;
                Debug.Log("▶ 활성화 상태 - True 영상 재생");
            }
            else
            {
                videoPlayer.clip = videoIfFalse;
                Debug.Log("▶ 비활성화 상태 - False 영상 재생");
            }

            videoPlayer.Play();
            hasPlayed = true;
        }
    }
}
