using UnityEngine;
using System.Collections;


public class ResettableObject : MonoBehaviour
{
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private bool initialActive;

    private MonoBehaviour[] scripts;

    void Awake()
    {
        // 초기 상태 저장
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        initialActive = gameObject.activeSelf;

        // 껐다 켰을 때 재작동되길 원하는 스크립트 목록
        scripts = GetComponents<MonoBehaviour>();
    }

    public void ResetObject()
{
    // 위치, 회전 복원
    transform.position = initialPosition;
    transform.rotation = initialRotation;

    // 콜라이더 잠깐 껐다 켜기 (트리거 상태 리셋용)
    StartCoroutine(ResetColliderTemporarily());

    // 부착된 스크립트의 OnReset() 호출
    foreach (var script in scripts)
    {
        var type = script.GetType();
        var method = type.GetMethod("OnReset");
        if (method != null)
        {
            method.Invoke(script, null);
        }
    }

    Debug.Log($"{gameObject.name} 리셋됨!");
}


private IEnumerator ResetColliderTemporarily()
{
    Collider2D col = GetComponent<Collider2D>();
    if (col != null)
    {
        col.enabled = false;
        yield return new WaitForSecondsRealtime(0.1f);
        col.enabled = true;
        Debug.Log("보스 콜라이더 재활성화");
    }
}

}
