public interface InventoryManagerInterface
{
    int GetItemCount(string itemName);
    void AddItem(string itemName);
    void UseItem(string itemName);
    bool HasItem(string itemName);
}
