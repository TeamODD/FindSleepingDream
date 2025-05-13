using UnityEngine;

public class PrisonLock : MonoBehaviour, IInteractable
{
    public InventoryManager inventoryManager;

    public void Interact()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null && inventoryManager != null)
        {
            if (inventoryManager.HasItem("Key"))
            {
                inventoryManager.UseItem("Key");
                Debug.Log("[PrisonLock] 열쇠 사용됨, 감옥 열림!");
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("[PrisonLock] 열쇠가 없어 문을 열 수 없습니다.");
            }
        }
        else
        {
            Debug.LogWarning("[PrisonLock] 플레이어나 인벤토리 매니저가 없습니다.");
        }
    }
}
