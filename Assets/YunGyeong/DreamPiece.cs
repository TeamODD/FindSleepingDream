using UnityEngine;

public class DreamPiece : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            var inventory = player.GetComponent<PlayerMove>();
            if (inventory != null)
            {
                inventory.AddDreamShard(); // 여기서 dreamShardCount 증가시킴
            }
        }

        Destroy(gameObject);
    }
}