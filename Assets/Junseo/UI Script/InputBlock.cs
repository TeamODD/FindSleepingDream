using UnityEngine;

public class InputBlock : MonoBehaviour
{
    public float blockDuration = 0f; // 0이면 닿는 동안, >0이면 일시 차단

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<PlayerMove>();
            if (player == null) return;

            if (blockDuration > 0f)
                player.BlockInputForSeconds(blockDuration);
            else
                player.SetInputBlocked(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (blockDuration == 0f && other.CompareTag("Player"))
        {
            var player = other.GetComponent<PlayerMove>();
            player?.SetInputBlocked(false);
        }
    }
}
