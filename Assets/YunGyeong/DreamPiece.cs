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
                inventory.AddDreamShard();
            }
        }

        // �ֽ� ������� �ƾ� �Ŵ��� ã�� (��� ���ŵ�!)
        CutsceneManager1 manager = Object.FindFirstObjectByType<CutsceneManager1>();
        if (manager != null)
        {
            Debug.Log("�ƾ� ����! index: ");
            manager.ShowCutscene();
        }
        else
        {
            Debug.LogWarning("CutsceneManager1 �� ã��!");
        }

        Destroy(gameObject);
    }
}
