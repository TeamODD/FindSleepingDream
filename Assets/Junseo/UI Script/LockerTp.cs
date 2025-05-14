using UnityEngine;

public class LockerTP : MonoBehaviour
{
    public GameObject targetObject;     // 이동시킬 대상 오브젝트
    public float destinationX;          // 이동할 x 위치

    private bool isPlayerInTrigger = false;

    void Update()
    {
        if (isPlayerInTrigger && Input.GetKeyDown(KeyCode.C))
        {
            if (targetObject != null)
            {
                Vector3 currentPosition = targetObject.transform.position;
                targetObject.transform.position = new Vector3(destinationX, currentPosition.y, currentPosition.z);
                Debug.Log("오브젝트 X 위치로 이동 완료");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = false;
        }
    }
}
