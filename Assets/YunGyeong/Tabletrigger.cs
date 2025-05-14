using UnityEngine;

public class Tabletrigger : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            ChaseAnimationChange chaseAnimationChange = collision.gameObject.GetComponent<ChaseAnimationChange>();
            chaseAnimationChange.SetChase();
        }
    }
}