using UnityEngine;

public class ChaseAnimationChange : MonoBehaviour
{
    private Animator animator;
    private bool IsChase = false;

    public Transform player;          //  플레이어 트랜스폼 연결
    public float stopChaseX = 286f;     //   좌표 기준 (ex: 0 이하로 가면 원복)

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (IsChase && player.position.x > stopChaseX)
        {
            IsChase = false;
            animator.SetBool("IsChase", false);
            Debug.Log("[ChaseAnimationChange] 추격 종료: 원복됨");
        }
    }

    public void SetChase()
    {
        IsChase = true;
        animator.SetBool("IsChase", true);
        Debug.Log("[ChaseAnimationChange] 추격 시작!");
    }
}
