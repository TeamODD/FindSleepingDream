using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static bool IsPaused = false;

    public GameObject pausePanel;
    public Slider volumeSlider;

    void Start()
    {
        pausePanel.SetActive(false);
        volumeSlider.onValueChanged.AddListener(SetVolume);
        AudioListener.volume = volumeSlider.value;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        IsPaused = !IsPaused;
        pausePanel.SetActive(IsPaused);
        Time.timeScale = IsPaused ? 0f : 1f;
    }

    public void ResumeGame()
    {
        TogglePause();
    }

    public void SetVolume(float value)
    {
        AudioListener.volume = value;
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
