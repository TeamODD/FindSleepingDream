using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioSource bgmSource;
    public AudioSource sfxSource;

    public AudioClip bgmClip;
    public AudioClip walkClip;
    public AudioClip runClip;

    public float walkInterval = 0.5f;
    public float runInterval = 0.25f;

    private float nextStepTime;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    void Start()
    {
        bgmSource.loop = true;
        bgmSource.clip = bgmClip;
        bgmSource.Play();
    }

    public void PlayStepSound(bool isRunning)
    {
        if (Time.time < nextStepTime) return;

        var clip = isRunning ? runClip : walkClip;
        var interval = isRunning ? runInterval : walkInterval;

        if (clip != null)
        {
            sfxSource.PlayOneShot(clip);
            nextStepTime = Time.time + interval;
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    void OnApplicationQuit()
    {
        bgmSource.Stop();
        sfxSource.Stop();
        AudioListener.volume = 0f; // 종료 시 소리 차단
    }
}
