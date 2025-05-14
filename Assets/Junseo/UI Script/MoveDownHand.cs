using UnityEngine;
using System.Collections;

public class MoveDownHand1 : MonoBehaviour
{
    public Vector3 moveOffset = new Vector3(2f, 0f, 0f); // 앞으로 이동 거리
    public float moveDuration = 1f;  // 이동 시간
    public float waitDuration = 8f;  // 뒤에서 대기 시간

    private Vector3 startPos;
    private Vector3 targetPos;
    private float timer = 0f; // 타이머로 moveDuration, waitDuration 작동시키기 용도.

    private enum State { MovingForward, MovingBackward, Waiting } // 숫자를 이름으로 변경.

    private State currentState = State.MovingForward; 
    private bool started = false;

    void Start()
    {
        startPos = transform.localPosition;
        targetPos = startPos + moveOffset;
        StartCoroutine(DelayStart());

    }

    IEnumerator DelayStart()
    {
        yield return new WaitForSeconds(4f);
        started = true;
    }

    void Update()
    {
        if (!started) return; 
        timer += Time.deltaTime;

        switch (currentState)
        {
            case State.MovingForward:
                float tF = timer / moveDuration;
                transform.localPosition = Vector3.Lerp(startPos, targetPos, tF);
                if (timer >= moveDuration)
                {
                    timer = 0f;
                    currentState = State.MovingBackward;
                }
                break;

            case State.MovingBackward:
                float tB = timer / moveDuration;
                transform.localPosition = Vector3.Lerp(targetPos, startPos, tB);
                if (timer >= moveDuration)
                {
                    timer = 0f;
                    currentState = State.Waiting;
                }
                break;

            case State.Waiting:
                if (timer >= waitDuration)
                {
                    timer = 0f;
                    currentState = State.MovingForward;
                }
                break;
        }
    }
}