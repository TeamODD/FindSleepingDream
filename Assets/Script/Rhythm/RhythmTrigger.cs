using UnityEngine;
using System.Collections;

public class RhythmTrigger : MonoBehaviour
{
    public Transform player;
    public GameObject rhythmGame;
    public float triggerX = 50f;

    private bool triggered = false;

    void Update()
    {
        if (!triggered && player.position.x >= triggerX)
        {
            triggered = true;
            StartCoroutine(ActivateRhythmGameWithDelay());
        }
    }

    private IEnumerator ActivateRhythmGameWithDelay()
    {
        rhythmGame.SetActive(true); // 1. 오브젝트 먼저 활성화
        yield return new WaitForSeconds(1f); // 2. 1초 대기

        Rhythm rhythm = rhythmGame.GetComponent<Rhythm>();
        if (rhythm != null)
        {
            rhythm.StartGame(); // 3. 게임 시작
        }
    }
}
