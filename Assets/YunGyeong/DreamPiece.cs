using UnityEngine;

public class DreamPiece : MonoBehaviour, IInteractable
{
    public int[] cutsceneIndices; // ✅ 여러 컷씬 인덱스 입력 가능

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

        CutsceneManager manager = FindFirstObjectByType<CutsceneManager>();
        if (manager != null)
        {
            Debug.Log("컷씬 시퀀스 실행! indices: " + string.Join(", ", cutsceneIndices));
            manager.ShowCutsceneSequence(cutsceneIndices); // ✅ 배열로 넘김
        }
        else
        {
            Debug.LogWarning("CutsceneManager 를 찾을 수 없음!");
        }

        Destroy(gameObject);
    }
}
