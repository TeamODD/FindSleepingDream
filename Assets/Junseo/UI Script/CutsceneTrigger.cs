using UnityEngine;

public class CutsceneTrigger : MonoBehaviour
{
    public int[] cutsceneIndices;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        foreach (int index in cutsceneIndices)
        {
            Debug.Log($"🎬 컷씬 {index} 실행 (무제한 반복 가능)");
            FindFirstObjectByType<CutsceneManager>()?.ShowCutsceneSequence(index);
        }
    }
}
