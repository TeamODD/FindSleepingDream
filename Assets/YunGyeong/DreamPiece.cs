using UnityEngine;

public class DreamPiece : MonoBehaviour, IInteractable
{
    public int cutsceneIndex = 0; // 이 드림피스에 연결될 컷씬 인덱스

    public void Interact()
    {
        Debug.Log("DreamPiece: Interact() 호출됨!");
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            var inventory = player.GetComponent<PlayerMove>();
            if (inventory != null)
            {
                inventory.AddDreamShard();
            }
        }

        // CutsceneManager로 컷씬 실행
        CutsceneManager manager = Object.FindFirstObjectByType<CutsceneManager>();
        if (manager != null)
        {
            Debug.Log("컷씬 실행! index: " + cutsceneIndex);
            manager.ShowCutsceneSequence(cutsceneIndex);
        }
        else
        {
            Debug.LogWarning("CutsceneManager 를 찾을 수 없음!");
        }

        Destroy(gameObject); // 드림피스 제거
    }
}
