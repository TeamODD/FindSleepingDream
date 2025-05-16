using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class RenderTextureCleaner : MonoBehaviour
{
    public RenderTexture renderTexture;
    public GameObject rawImageObject;
    public VideoPlayer videoPlayer;
    public string sceneToLoad;

    void Start()
    {
        Debug.Log("[RenderTextureCleaner][Start] 초기화됨");

        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached += OnVideoEnd;

            if (videoPlayer.clip == null)
            {
                Debug.LogWarning("🚫 영상이 비어 있어 이벤트 연결 생략");
                videoPlayer.loopPointReached -= OnVideoEnd;
            }
            else
            {
                Debug.Log("📎 영상 종료 시 OnVideoEnd 연결됨");
            }
        }
    }

    void OnDisable()
    {
        Debug.Log("[RenderTextureCleaner] OnDisable 호출됨");
        CleanUp();
    }

    void OnApplicationQuit()
    {
        Debug.Log("[RenderTextureCleaner] OnApplicationQuit 호출됨");
        CleanUp();
    }

    void OnVideoEnd(VideoPlayer vp)
{
    if (vp.clip == null)
    {
        Debug.LogWarning("❌ 영상이 null인데 종료 이벤트 호출됨 - 무시");
        return;
    }

    Debug.Log("🎬 영상 종료됨 → 씬 이동 시작!");

    if (string.IsNullOrEmpty(sceneToLoad))
    {
        Debug.LogError("❌ sceneToLoad 값이 비어 있음! 씬 이동 실패");
        return;
    }

    CleanUp();
    Debug.Log($"🚪 SceneManager.LoadScene(\"{sceneToLoad}\") 호출");
    SceneManager.LoadScene(sceneToLoad);
}


    void CleanUp()
    {
        Debug.Log("🧹 CleanUp() 호출됨");

        if (renderTexture != null)
        {
            RenderTexture currentRT = RenderTexture.active;
            RenderTexture.active = renderTexture;
            GL.Clear(true, true, Color.clear);
            RenderTexture.active = currentRT;

            Debug.Log("✅ RenderTexture 클리어 완료");
        }

        if (rawImageObject != null)
        {
            rawImageObject.SetActive(false);
            Debug.Log("✅ RawImage 오브젝트 비활성화 완료");
        }
    }
}
