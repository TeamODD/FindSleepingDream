using UnityEngine;

public class ChaseAnimationChange : MonoBehaviour
{
    private Animator animator;
    private bool IsChase = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // 아직 추격 상태가 아니고, x 좌표가 0 초과면 추격 모드로 전환
        //if (!IsChase && Input.GetKeyDown(KeyCode.Z))
        //{
        //    IsChase = true;
        //    animator.SetBool("IsChase", true);
        //    Debug.Log("[PlayerMove] 추격 애니메이션 상태 진입!");
        //}
        if (IsChase && Input.GetKeyDown(KeyCode.C))
        {
            IsChase = false;
            animator.SetBool("IsChase", false);
        }
    }

    public void SetChase()
    {
        IsChase = true;
        animator.SetBool("IsChase", true);
    }
}