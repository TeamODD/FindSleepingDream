using UnityEngine;
public class ItemTester : MonoBehaviour
{
    public InventoryManager inventory;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            inventory.AddItem("Key");

        if (Input.GetKeyDown(KeyCode.Alpha2))
            inventory.AddItem("Blanket");

        if (Input.GetKeyDown(KeyCode.Alpha3))
            inventory.AddItem("Star");

        if (Input.GetKeyDown(KeyCode.Alpha4))
            inventory.UseItem("Blanket");

        if (Input.GetKeyDown(KeyCode.Alpha5))
            inventory.UseItem("Key");
    }
}
