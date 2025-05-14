using UnityEngine;

public class AfterCutscene : MonoBehaviour
{
    public int triggerCutsceneIndex;
    public GameObject targetObject;
    public float targetX;

    public void OnCutsceneEnded(int endedCutsceneIndex)
    {
        if (endedCutsceneIndex == triggerCutsceneIndex && targetObject != null)
        {
            Vector3 currentPos = targetObject.transform.position;
            targetObject.transform.position = new Vector3(targetX, currentPos.y, currentPos.z);
            Debug.Log($"컷씬 {endedCutsceneIndex} 끝나고 {targetObject.name} 이동!");
        }
    }
}
