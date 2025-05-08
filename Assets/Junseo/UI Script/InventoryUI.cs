using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public Image slotKey;
    public Image slotBlanket;
    public Image slotStar1;
    public Image slotStar2;
    public Image slotStar3;

    void Start()
    {
        slotKey.enabled = false;
        slotBlanket.enabled = false;
        slotStar1.enabled = false;
        slotStar2.enabled = false;
        slotStar3.enabled = false;
    }


    // �ܺο��� ȣ���Ͽ� ���� ���¿� �°� UI ����
    public void UpdateUI(InventoryManagerInterface inventory)
    {
        // ����� ���� ���� ���θ� �ݿ�
        slotKey.enabled = inventory.HasItem("Key");
        slotBlanket.enabled = inventory.HasItem("Blanket");

        // ������ ����ŭ ǥ��
        int starCount = inventory.GetItemCount("Star");
        slotStar1.enabled = starCount >= 1;
        slotStar2.enabled = starCount >= 2;
        slotStar3.enabled = starCount >= 3;
    }
}
