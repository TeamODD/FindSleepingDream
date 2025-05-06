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


    // 외부에서 호출하여 현재 상태에 맞게 UI 갱신
    public void UpdateUI(InventoryManagerInterface inventory)
    {
        // 열쇠와 담요는 보유 여부만 반영
        slotKey.enabled = inventory.HasItem("Key");
        slotBlanket.enabled = inventory.HasItem("Blanket");

        // 별조각 수만큼 표시
        int starCount = inventory.GetItemCount("Star");
        slotStar1.enabled = starCount >= 1;
        slotStar2.enabled = starCount >= 2;
        slotStar3.enabled = starCount >= 3;
    }
}
