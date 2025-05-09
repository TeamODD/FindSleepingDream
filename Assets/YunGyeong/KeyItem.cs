using UnityEngine;

public class KeyItem : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Debug.Log($"[KeyItem] 찾은 오브젝트 이름: {player?.name}");

        if (player != null)
        {
            var inventory = player.GetComponent<PlayerMove>();
            Debug.Log($"[KeyItem] PlayerMove 있음?: {inventory != null}");
            if (inventory != null)
            {
                Debug.Log("[KeyItem]  PlayerMove 컴포넌트 찾음!");
                inventory.CollectItem("Key");
            }
            else
            {
                Debug.LogWarning("[KeyItem]  PlayerMove 못 찾음!");
            }
        }
        else
        {
            Debug.LogWarning("[KeyItem]  Player 태그 오브젝트 자체를 못 찾음!");
        }

        Destroy(gameObject);
    }


}
