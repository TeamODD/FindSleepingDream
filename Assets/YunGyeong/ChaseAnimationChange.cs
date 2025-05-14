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
        // ���� �߰� ���°� �ƴϰ�, x ��ǥ�� 0 �ʰ��� �߰� ���� ��ȯ
        //if (!IsChase && Input.GetKeyDown(KeyCode.Z))
        //{
        //    IsChase = true;
        //    animator.SetBool("IsChase", true);
        //    Debug.Log("[PlayerMove] �߰� �ִϸ��̼� ���� ����!");
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