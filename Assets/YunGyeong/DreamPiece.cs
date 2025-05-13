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
                inventory.AddDreamShard(); // ���⼭ dreamShardCount ������Ŵ
            }
        }

        Destroy(gameObject);
    }
}