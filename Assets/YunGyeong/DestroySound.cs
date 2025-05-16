using UnityEngine;

public class DestroySound : MonoBehaviour
{
    public AudioClip destroyClip;

    private void OnDestroy()
    {
        // 파괴될 때 SFX 재생
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(destroyClip);
        }
    }
}
