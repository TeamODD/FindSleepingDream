using UnityEngine;

public class Items : MonoBehaviour
{
    public string itemName;

    public void Interact()
    {
        Debug.Log($"[ȹ��] {itemName} �������� ������ϴ�!");
        Destroy(gameObject); // ȹ�� �� ����
    }
}
