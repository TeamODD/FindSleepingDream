using UnityEngine;

public class Blanket : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            var inventory = player.GetComponent<PlayerMove>();
            if (inventory != null)
            {
                inventory.CollectItem("Blanket"); // 인벤토리에 "Blanket" 추가
            }
        }

        Destroy(gameObject); // 획득 후 사라지기
    }
}
