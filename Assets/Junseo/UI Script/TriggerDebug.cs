using UnityEngine;

public class TriggerDebug : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"[TriggerDebug] �� {other.name} ��(��) �浹 ����");
    }
}
