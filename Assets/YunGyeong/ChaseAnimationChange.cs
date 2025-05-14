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
        if (!IsChase && transform.position.x > 0f)
        {
            IsChase = false;
            animator.SetBool("IsChase", false);
            Debug.Log("[PlayerMove] �߰� �ִϸ��̼� ���� ����!");
        }
    }
}
