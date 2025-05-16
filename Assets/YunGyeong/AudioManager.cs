using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource bgmSource;
    public AudioSource sfxSource;

    [Header("Clips")]
    public AudioClip defaultBGM;
    public AudioClip walkClip;
    public AudioClip runClip;

    [Header("Step Timing")]
    public float walkInterval = 0.5f;
    public float runInterval = 0.25f;

    [Header("Master Volume Slider")]
    public Slider volumeSlider;  // 하나만 사용

    private float nextStepTime = 0f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayBGM(defaultBGM);
        LoadVolumeSettings();
        SetupSlider();
    }

    public void PlayBGM(AudioClip clip)
    {
        if (clip == null) return;
        bgmSource.clip = clip;
        bgmSource.loop = true;
        bgmSource.Play();
    }

    public void PlayStepSound(bool isRunning)
    {
        if (Time.time < nextStepTime) return;

        AudioClip stepClip = isRunning ? runClip : walkClip;
        float interval = isRunning ? runInterval : walkInterval;

        if (stepClip != null)
        {
            sfxSource.PlayOneShot(stepClip);
            nextStepTime = Time.time + interval;
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip != null)
            sfxSource.PlayOneShot(clip);
    }

    private void SetupSlider()
    {
        if (volumeSlider != null)
        {
            volumeSlider.onValueChanged.AddListener(v =>
            {
                AudioListener.volume = v;
                PlayerPrefs.SetFloat("MasterVolume", v);
            });
        }
    }

    private void LoadVolumeSettings()
    {
        float saved = PlayerPrefs.GetFloat("MasterVolume", 1f);
        AudioListener.volume = saved;
        if (volumeSlider != null)
            volumeSlider.value = saved;
    }
}
