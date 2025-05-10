using UnityEngine;
using UnityEngine.SceneManagement;

public class Mainmenu : MonoBehaviour
{
    public void StartGame()
    {
        Debug.Log("Start ��ư ����");
        SceneManager.LoadScene("Junseo");
    }

    public void QuitGame()
    {
        Debug.Log("Quit ��ư ����");
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}