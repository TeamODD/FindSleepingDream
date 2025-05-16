using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using UnityEngine.UI;

public class VideoManager : MonoBehaviour
{
    public GameObject targetObject;
    public VideoPlayer videoPlayer;
    public VideoClip videoIfExists;
    public VideoClip videoIfDestroyed;
    public GameObject rawImageObject;
    public RawImage rawImage;

    [Header("다음으로 이동할 씬 이름")]
    public string sceneToLoad = "MainMenu"; // Inspector에서 설정 가능

    private bool hasPlayed = false;

    private void Start()
    {
        Debug.Log("[VideoManager][Start] 시작");

        if (videoPlayer == null)
        {
            Debug.LogError("❌ VideoPlayer 연결 안 됨!");
            return;
        }

        Debug.Log($"[VideoManager] videoPlayer.isPlaying = {videoPlayer.isPlaying}");

        // ✨ 시작할 땐 clip이 없으므로 이벤트 연결 생략 (중복 방지)
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasPlayed) return;

        if (other.CompareTag("Player"))
        {
            Debug.Log("✅ Player와 충돌 감지 → 영상 준비 시작");

            videoPlayer.clip = (targetObject == null) ? videoIfDestroyed : videoIfExists;

            if (videoPlayer.clip == null)
            {
                Debug.LogError("🚫 재생할 영상이 null입니다. 실행 중지");
                return;
            }

            // ✅ 이벤트 연결
            videoPlayer.loopPointReached -= OnVideoEnd; // 중복 방지
            videoPlayer.loopPointReached += OnVideoEnd;

            Debug.Log($"🎬 재생할 영상: {videoPlayer.clip.name}");

            rawImage.texture = videoPlayer.targetTexture;
            rawImage.color = Color.white;
            rawImageObject.SetActive(true);

            StartCoroutine(PlayWhenPrepared());
            hasPlayed = true;
        }
    }

    private System.Collections.IEnumerator PlayWhenPrepared()
    {
        Debug.Log("🕒 영상 준비 중...");
        videoPlayer.Prepare();

        while (!videoPlayer.isPrepared)
            yield return null;

        Debug.Log("📼 영상 준비 완료 → 재생 시작");
        videoPlayer.Play();

        Debug.Log($"🎥 isPrepared: {videoPlayer.isPrepared}");
        Debug.Log($"▶️ isPlaying: {videoPlayer.isPlaying}");
    }

    private void OnVideoEnd(VideoPlayer vp)
    {
        if (vp.clip == null)
        {
            Debug.LogWarning("❌ 영상이 null인데 종료 이벤트 호출됨 - 무시");
            return;
        }

        Debug.Log("🎬 영상 종료됨 → 씬 이동 시작");

        // ✅ RawImage 숨기기
        rawImageObject.SetActive(false);

        // ✅ 씬 이동
        if (string.IsNullOrEmpty(sceneToLoad))
        {
            Debug.LogError("❌ 이동할 씬 이름이 비어 있음! 씬 이동 실패");
            return;
        }

        Debug.Log($"🚪 SceneManager.LoadScene(\"{sceneToLoad}\") 호출");
        SceneManager.LoadScene(sceneToLoad);
    }
}
