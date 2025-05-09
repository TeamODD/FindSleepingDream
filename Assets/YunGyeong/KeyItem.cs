using UnityEngine;

public class KeyItem : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Debug.Log($"[KeyItem] ã�� ������Ʈ �̸�: {player?.name}");

        if (player != null)
        {
            var inventory = player.GetComponent<PlayerMove>();
            Debug.Log($"[KeyItem] PlayerMove ����?: {inventory != null}");
            if (inventory != null)
            {
                Debug.Log("[KeyItem]  PlayerMove ������Ʈ ã��!");
                inventory.CollectItem("Key");
            }
            else
            {
                Debug.LogWarning("[KeyItem]  PlayerMove �� ã��!");
            }
        }
        else
        {
            Debug.LogWarning("[KeyItem]  Player �±� ������Ʈ ��ü�� �� ã��!");
        }

        Destroy(gameObject);
    }


}
