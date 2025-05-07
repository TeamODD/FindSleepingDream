using UnityEngine;

public class KeyItem : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            var inventory = player.GetComponent<PlayerMove>();
            if (inventory != null)
            {
                inventory.CollectItem("Key");
                Debug.Log("ø≠ºË∏¶ »πµÊ«ﬂΩ¿¥œ¥Ÿ!");
            }
        }

        Destroy(gameObject);
    }
}
