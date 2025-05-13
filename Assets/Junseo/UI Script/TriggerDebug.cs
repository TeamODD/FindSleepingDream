using UnityEngine;

public class TriggerDebug : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"[TriggerDebug] ▶ {other.name} 이(가) 충돌 진입");
    }
}
