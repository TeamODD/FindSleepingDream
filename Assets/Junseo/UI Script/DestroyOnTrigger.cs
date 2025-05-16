using UnityEngine;

public class DisableOnTrigger : MonoBehaviour
{
    [Tooltip("한 번 닿으면 꺼질 오브젝트입니다.")]
    public string targetTag = "Player";  // 기본은 Player 태그

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(targetTag))
        {
            gameObject.SetActive(false);
            Debug.Log($"{gameObject.name} : {targetTag}와 충돌 후 비활성화됨");
        }
    }
}
