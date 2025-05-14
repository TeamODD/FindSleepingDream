using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Direction
{
    Up, Down, Left, Right
}


public class MyCutsceneTrigger : MonoBehaviour
{
    [Tooltip("이 트리거에서 실행할 컷씬 번호")]
    public int cutsceneIndex = 0;

    private bool hasPlayed = false;

    public void OnReset()
    {
        hasPlayed = false;
        Debug.Log("컷씬 트리거 리셋됨!");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!hasPlayed && other.CompareTag("Player"))
        {
            FindFirstObjectByType<CutsceneManager>()?.ShowCutsceneSequence(cutsceneIndex);
            hasPlayed = true;
        }
    }
}

public class Rhythm : MonoBehaviour
{
    public GameObject[] circles;
    public Sprite upArrow, downArrow, leftArrow, rightArrow;
    public Sprite PressedUp, PressedDown, PressedLeft, PressedRight;

    public GameObject playerToMove;          // 성공 시 이동 대상
    public float successTargetX = 0f;        // 성공 시 X 위치
    public int failureCutsceneIndex = 0;     // 실패 시 컷씬 인덱스

    private List<Direction> directions = new List<Direction>();
    private int currentIndex = 0;
    private bool inputEnabled = false;
    private int round = 0;

    private CutsceneManager cutsceneManager;

    public void OnReset()
    {
        StopAllCoroutines();
        inputEnabled = false;
        round = 0;
        currentIndex = 0;

        ResetCircleSprites();

        foreach (GameObject arrow in circles)
            arrow.SetActive(false);

        Debug.Log("🎵 Rhythm 리셋 완료");

        // 게임 자동 시작
        StartGame();
    }

    private void Awake()
    {
        // 시작 시 화살표 UI 숨기기
        foreach (GameObject arrow in circles)
        {
            arrow.SetActive(false);
        }

        cutsceneManager = FindObjectOfType<CutsceneManager>();
    }

    public void StartGame()
    {
        foreach (GameObject arrow in circles)
        {
            arrow.SetActive(true);
        }

        StartCoroutine(GameRoutine());
    }


    private IEnumerator GameRoutine()
{
    for (round = 1; round <= 3; round++)
    {
        Debug.Log($"라운드 {round} 시작!");

        currentIndex = 0;
        GenerationDirections();
        ResetCircleSprites();
        DisplayDirections();
        inputEnabled = true;

        float timer = 5f;
        while (timer > 0f && currentIndex < directions.Count)
        {
            timer -= Time.deltaTime;
            yield return null;
        }

        inputEnabled = false;

        if (currentIndex >= directions.Count)
        {
            Debug.Log("라운드 성공!");
            yield return new WaitForSeconds(0.2f);
        }
        else
        {
            Debug.Log("라운드 실패!");

            // 화살표 숨기기
            foreach (GameObject arrow in circles)
                arrow.SetActive(false);

            // 컷씬 즉시 실행
            if (cutsceneManager != null)
                cutsceneManager.ShowCutsceneSequence(failureCutsceneIndex);

            // Rhythm 오브젝트는 잠시 후에 끄기
            StartCoroutine(DeactivateSelfLater());

            yield break; // 게임 종료
        }
    }

    // 🎉 모든 라운드 성공했을 때만 도달함
    Debug.Log("모든 라운드 성공! 순간이동 실행");

    // 화살표 숨기기
    foreach (GameObject arrow in circles)
        arrow.SetActive(false);

    // 순간이동
    if (playerToMove != null)
    {
        Vector3 current = playerToMove.transform.position;
        playerToMove.transform.position = new Vector3(successTargetX, current.y, current.z);
    }

    // Rhythm 오브젝트는 잠시 후에 끄기
    StartCoroutine(DeactivateSelfLater());
}


    private IEnumerator DeactivateSelfLater()
    {
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }

    

    private void Update()
    {
        if (!inputEnabled) return;

        if (Input.GetKeyDown(KeyCode.UpArrow)) CheckInput(Direction.Up);
        if (Input.GetKeyDown(KeyCode.DownArrow)) CheckInput(Direction.Down);
        if (Input.GetKeyDown(KeyCode.LeftArrow)) CheckInput(Direction.Left);
        if (Input.GetKeyDown(KeyCode.RightArrow)) CheckInput(Direction.Right);
    }

    private void CheckInput(Direction direction)
    {
        if (!inputEnabled || currentIndex >= directions.Count) return;

        StartCoroutine(Changer(currentIndex, direction));

        if (direction == directions[currentIndex])
        {
            currentIndex++;
        }
        else
        {
            inputEnabled = false;
            Debug.Log("입력 실패!");

            foreach (GameObject arrow in circles)
                arrow.SetActive(false);

            if (cutsceneManager != null)
                cutsceneManager.ShowCutsceneSequence(failureCutsceneIndex);

            StartCoroutine(DeactivateSelfLater());

        }
    }



    private void GenerationDirections()
    {
        directions.Clear();
        for (int i = 0; i < 5; i++)
        {
            directions.Add((Direction)Random.Range(0, 4));
        }
    }

    private void DisplayDirections()
    {
        for (int i = 0; i < circles.Length; i++)
        {
            Image img = circles[i].GetComponent<Image>();
            switch (directions[i])
            {
                case Direction.Up: img.sprite = upArrow; break;
                case Direction.Down: img.sprite = downArrow; break;
                case Direction.Left: img.sprite = leftArrow; break;
                case Direction.Right: img.sprite = rightArrow; break;
            }

            Debug.Log($"[Rhythm] {i}번 화살표 설정 완료: {directions[i]}");
        }
    }

    private void ResetCircleSprites()
    {
        for (int i = 0; i < circles.Length; i++)
        {
            Image img = circles[i].GetComponent<Image>();
            img.sprite = null;
        }
    }

    private IEnumerator Changer(int index, Direction dir)
    {
        Image img = circles[index].GetComponent<Image>();

        switch (dir)
        {
            case Direction.Up: img.sprite = PressedUp; break;
            case Direction.Down: img.sprite = PressedDown; break;
            case Direction.Left: img.sprite = PressedLeft; break;
            case Direction.Right: img.sprite = PressedRight; break;
        }

        yield return new WaitForSeconds(0.2f);

        switch (dir)
        {
            case Direction.Up: img.sprite = upArrow; break;
            case Direction.Down: img.sprite = downArrow; break;
            case Direction.Left: img.sprite = leftArrow; break;
            case Direction.Right: img.sprite = rightArrow; break;
        }
    }
}
