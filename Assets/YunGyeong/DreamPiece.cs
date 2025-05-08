using UnityEngine;

public class DreamPiece : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            var inventory = player.GetComponent<PlayerMove>();
            if (inventory != null)
            {
                inventory.AddDreamShard();
            }
        }

        // 최신 방식으로 컷씬 매니저 찾기 (경고 제거됨!)
        CutsceneManager1 manager = Object.FindFirstObjectByType<CutsceneManager1>();
        if (manager != null)
        {
            Debug.Log("컷씬 실행! index: ");
            manager.ShowCutscene();
        }
        else
        {
            Debug.LogWarning("CutsceneManager1 못 찾음!");
        }

        Destroy(gameObject);
    }
}
