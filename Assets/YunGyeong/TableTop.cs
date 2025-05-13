using UnityEngine;

public class TableTop : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerTableStun stunComp = collision.gameObject.GetComponent<PlayerTableStun>();
            if (stunComp != null && !stunComp.IsCrouching())
            {
                stunComp.TriggerStun(1f);
            }
        }
    }
}
