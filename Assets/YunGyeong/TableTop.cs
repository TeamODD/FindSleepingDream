using UnityEngine;

public class TableTop : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerTableStun stunComp = other.GetComponent<PlayerTableStun>();
            if (stunComp != null && !stunComp.IsCrouching())
            {
                stunComp.TriggerStun(0.5f);
            }
        }
    }
}
