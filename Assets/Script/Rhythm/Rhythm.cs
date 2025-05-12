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
    private List<Direction> directions = new List<Direction>();
    private int currentIndex = 0;
    private bool inputEnabled = false;

    private bool isPlaying = false;
    private int round = 0;

    private void Start()
    {
        StartCoroutine(GameRoutine());
    }

    private IEnumerator GameRoutine()
    {
        for (round = 1; round <= 3; round++)
        {
            Debug.Log("시작!");

            currentIndex = 0;
            GenerationDirections();
            ResetCircleSprites(); // <-- 여기 추가
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
                Debug.Log("성공!");
            }
            else
            {
                Debug.Log("시간초과!");
            }

            yield return new WaitForSeconds(0.2f); // 다음 라운드 전 대기 시간
        }

        Debug.Log("게임 끝!");
    }


    private void ResetCircleSprites()
    {
        for (int i = 0; i < circles.Length; i++)
        {
            Image img = circles[i].GetComponent<Image>();
            img.sprite = null; // 또는 디폴트 빈 이미지
        }
    }

    private void Update()
    {
       

        if (!inputEnabled) return;

        if (Input.GetKeyDown(KeyCode.UpArrow)) CheckInput(Direction.Up);
        if (Input.GetKeyDown(KeyCode.DownArrow)) CheckInput(Direction.Down);
        if (Input.GetKeyDown(KeyCode.LeftArrow)) CheckInput(Direction.Left);
        if (Input.GetKeyDown(KeyCode.RightArrow)) CheckInput(Direction.Right);


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
        }
    }




    private IEnumerator Changer(int index, Direction dir)
    {
        Image img = circles[index].GetComponent<Image>();

        // 눌림 상태로 바꾸기
        switch (dir)
        {
            case Direction.Up: img.sprite = PressedUp; break;
            case Direction.Down: img.sprite = PressedDown; break;
            case Direction.Left: img.sprite = PressedLeft; break;
            case Direction.Right: img.sprite = PressedRight; break;
        }
        //for (int i = 0; i<=2; i++)
        yield return new WaitForSeconds(0.2f); // 0.2초 동안 눌린 상태 유지. 시간 너무 길게하면 오류남

        // 다시 원래 이미지로 복구
        switch (dir)
        {
            case Direction.Up: img.sprite = upArrow; break;
            case Direction.Down: img.sprite = downArrow; break;
            case Direction.Left: img.sprite = leftArrow; break;
            case Direction.Right: img.sprite = rightArrow; break;
        }
    }

    private void CheckInput(Direction direction)
    {
        if (!inputEnabled || currentIndex >= directions.Count)
            return;

        StartCoroutine(Changer(currentIndex, direction));

        if (direction == directions[currentIndex])
        {
            currentIndex++;
        }
        else
        {
            inputEnabled = false;
            Debug.Log("실패!");
        }
    }

    //private void CheckInput(Direction direction) // 라운드 클리어 유무 코드
    //{
    //    StartCoroutine(Changer(currentIndex, direction));

    //    if (direction == directions[currentIndex])
    //    {
    //        //circles[currentIndex].gameObject.SetActive(false);
    //        currentIndex++;
    //        if (currentIndex < directions.Count)
    //        {
    //            inputEnabled = false;
    //            Debug.Log("시간초과!");
    //        }
    //        else
    //        {
    //            Debug.Log("성공!");
    //        }
    //    }

    //}

    //private IEnumerator timer()
    //{
    //    inputEnabled = true;
    //    yield return new WaitForSeconds(10f);
    //    if (currentIndex < directions.Count)
    //    {
    //        inputEnabled = false;
    //        Debug.Log("시간초과!");
    //    }
    //}
}