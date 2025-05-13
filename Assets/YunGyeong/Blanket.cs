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
                inventory.CollectItem("Blanket"); // �κ��丮�� "Blanket" �߰�
            }
        }

        Destroy(gameObject); // ȹ�� �� �������
    }
}
