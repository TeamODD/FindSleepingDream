using UnityEngine;

public class GameOverResetManager : MonoBehaviour
{
    [Tooltip("이 컷씬 번호가 종료되면 리셋 실행")]
    public int targetCutsceneIndex;

    [Tooltip("초기화할 대상 오브젝트 (예: 보스)")]
    public GameObject objectToReset;

    public void OnCutsceneEnded(int cutsceneIndex)
    {
        if (cutsceneIndex == targetCutsceneIndex && objectToReset != null)
        {
            var resettable = objectToReset.GetComponent<ResettableObject>();
            if (resettable != null)
            {
                resettable.ResetObject();
                Debug.Log($"컷씬 {cutsceneIndex} 종료됨 → {objectToReset.name} 리셋 완료!");
            }
        }
    }
}
