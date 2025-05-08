using UnityEngine;
using UnityEngine.UI;

public class CutsceneManager1 : MonoBehaviour
{
    public GameObject[] Images;
    private int index = 0;

    private bool isShowing = false;

    public void ShowCutscene()
    {
        Images[index].gameObject.SetActive(true);
        isShowing = true;
    }

    private void Update()
    {
        if (isShowing && Input.GetKeyDown(KeyCode.F))
        {
            isShowing = false;
            Images[index].gameObject.SetActive(false);
            index++;

        }
    }
}
