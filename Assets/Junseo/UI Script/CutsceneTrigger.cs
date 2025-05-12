using UnityEngine;

public class CutsceneTriggerSequence : MonoBehaviour
{
    public int[] cutsceneIndices;           // 🔥 Inspector에서 설정
    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered || !other.CompareTag("Player")) return;
        triggered = true;

        var manager = FindFirstObjectByType<CutsceneManager>();
        if (manager != null)
        {
            manager.ShowCutsceneSequence(cutsceneIndices);
        }
    }
}
