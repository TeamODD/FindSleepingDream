using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Direction
{
    Up, Down, Left, Right
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
        bool allRoundsSuccess = true;

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
                yield return new WaitForSeconds(0.2f); // 다음 라운드 전 대기
            }
            else
            {
                Debug.Log("라운드 실패!");

                allRoundsSuccess = false;

                // 컷씬 즉시 실행
                if (cutsceneManager != null)
                {
                    cutsceneManager.ShowCutsceneSequence(failureCutsceneIndex);
                }

                break; // 게임 종료
            }
        }

        // 화살표 UI 숨기기
        foreach (GameObject arrow in circles)
        {
            arrow.SetActive(false);
        }

        // 모든 라운드를 성공했을 때만 이동
        if (allRoundsSuccess && playerToMove != null)
        {
            Vector3 current = playerToMove.transform.position;
            playerToMove.transform.position = new Vector3(successTargetX, current.y, current.z);
        }

        Debug.Log("게임 끝!");
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
