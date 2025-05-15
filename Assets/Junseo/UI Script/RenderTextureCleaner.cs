using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class RenderTextureCleaner : MonoBehaviour
{
    public RenderTexture renderTexture;
    public GameObject rawImageObject;
    public VideoPlayer videoPlayer;
    public string sceneToLoad; // 이동할 씬 이름

    void Start()
    {
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached += OnVideoEnd;
        }
    }

    void OnDisable()
    {
        CleanUp();
    }

    void OnApplicationQuit()
    {
        CleanUp();
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        Debug.Log("🎬 영상 끝! 씬 이동 시작");
        CleanUp();  // 먼저 화면 정리하고
        SceneManager.LoadScene(sceneToLoad);  // 씬 이동
    }

    void CleanUp()
    {
        if (renderTexture != null)
        {
            RenderTexture currentRT = RenderTexture.active;
            RenderTexture.active = renderTexture;
            GL.Clear(true, true, Color.clear);
            RenderTexture.active = currentRT;

            Debug.Log("🧹 RenderTexture 클리어됨");
        }

        if (rawImageObject != null)
        {
            rawImageObject.SetActive(false);
            Debug.Log("🧼 RawImage 숨김");
        }
    }
}
