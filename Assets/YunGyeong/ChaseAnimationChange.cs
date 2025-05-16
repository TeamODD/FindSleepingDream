using UnityEngine;

public class ChaseAnimationChange : MonoBehaviour
{
    private Animator animator;
    private bool IsChase = false;

    public Transform player;          //  �÷��̾� Ʈ������ ����
    public float stopChaseX = 286f;     //   ��ǥ ���� (ex: 0 ���Ϸ� ���� ����)

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
            Debug.Log("[ChaseAnimationChange] �߰� ����: ������");
        }
    }

    public void SetChase()
    {
        IsChase = true;
        animator.SetBool("IsChase", true);
        Debug.Log("[ChaseAnimationChange] �߰� ����!");
    }
}
