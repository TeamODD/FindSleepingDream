using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour, InventoryManagerInterface
{
    private Dictionary<string, int> items = new Dictionary<string, int>();

    [SerializeField] private InventoryUI inventoryUI; // UI 연결

    public int GetItemCount(string itemName)
    {
        return items.ContainsKey(itemName) ? items[itemName] : 0;
    }

    public bool HasItem(string itemName)
    {
        return GetItemCount(itemName) > 0;
    }

    public void AddItem(string itemName)
    {
        if (!items.ContainsKey(itemName))
            items[itemName] = 0;

        items[itemName]++;

        UpdateUI(); // 추가 시 UI 갱신
    }

    public void UseItem(string itemName)
    {
        if (HasItem(itemName))
        {
            items[itemName]--;
            UpdateUI(); // 사용 시 UI 갱신
        }
    }

    private void UpdateUI()
    {
        if (inventoryUI != null)
            inventoryUI.UpdateUI(this);
    }
}
