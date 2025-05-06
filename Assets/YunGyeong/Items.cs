using UnityEngine;

public class Items : MonoBehaviour
{
    public string itemName;

    public void Interact()
    {
        Debug.Log($"[획득] {itemName} 아이템을 얻었습니다!");
        Destroy(gameObject); // 획득 후 제거
    }
}
