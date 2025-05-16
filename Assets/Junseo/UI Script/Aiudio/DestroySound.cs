using UnityEngine;

public class DestroySound : MonoBehaviour
{
    public AudioClip destroyClip;

    void OnDestroy()
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlaySFX(destroyClip);
    }
}
