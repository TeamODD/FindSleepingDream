using UnityEngine;
using System.Collections;

public class MoveUpHand : MonoBehaviour
{
    public Vector3 moveOffset = new Vector3(2f, 0f, 0f); // 이동 거리
    public float moveDuration = 0;      // 이동 시간
    public float rotateDuration = 0.5f;  // 회전 시간
    public float waitDuration = 8f;      // 대기 시간
    public float rotateAngle = -45f;     // 회전 각도. inspector 에서 수정해야 적용됨. 

    private Vector3 startPos;
    private Vector3 targetPos;
    private Quaternion startRot;
    private Quaternion targetRot;

    void Start()
    {
        startPos = transform.localPosition;
        targetPos = startPos + moveOffset;

        startRot = transform.localRotation;
        targetRot = startRot * Quaternion.Euler(0f, 0f, rotateAngle);

        StartCoroutine(Sequence());
    }

    IEnumerator Sequence()
    {
        yield return new WaitForSeconds(11f); // 시작 대기

        while (true)
        {
            // 앞으로 이동
            yield return MoveToPosition(startPos, targetPos, moveDuration);

            // 회전
            yield return RotateTo(targetRot, rotateDuration);

            // 회전 원복 (X좌표는 이동한 채로 회전만 원위치로)
            yield return RotateTo(startRot, rotateDuration);

            // 뒤로 이동
            yield return MoveToPosition(targetPos, startPos, moveDuration);

            // 대기
            yield return new WaitForSeconds(waitDuration);
        }
    }

    IEnumerator MoveToPosition(Vector3 from, Vector3 to, float duration)
    {
        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / duration);
            transform.localPosition = Vector3.Lerp(from, to, t);
            yield return null;
        }
    }

    IEnumerator RotateTo(Quaternion target, float duration)
    {
        Quaternion from = transform.localRotation;
        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / duration);
            transform.localRotation = Quaternion.Lerp(from, target, t);
            yield return null;
        }
    }
}