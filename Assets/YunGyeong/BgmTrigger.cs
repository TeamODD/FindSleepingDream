using UnityEngine;

public class BGMTrigger : MonoBehaviour
{
    public AudioClip newBGM;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && newBGM != null)
        {
            AudioManager.Instance.PlayBGM(newBGM);
        }
    }
}
