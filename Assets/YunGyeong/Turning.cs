using UnityEngine;

public class Turning : MonoBehaviour
{
    public Transform playerTransform;
    public float triggerX = 60.03f; // �պ���
    public float resetX = 64f;   // �ڵ���
    private Animator animator;
    private bool isFacingFront = false; // ���� �� ���� �ִ� ����

    void Start()
    {
        animator = GetComponent<Animator>();
        if (playerTransform == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null) playerTransform = player.transform;
        }
    }

    void Update()
    {
        if (playerTransform == null) return;

        float playerX = playerTransform.position.x;

        // 0.66 ���� ���� �պ��� (���� �� ���� �ִ� ��츸)
        if (!isFacingFront && playerX >= triggerX && playerX < 62f)
        {
            animator.SetTrigger("Front");
            isFacingFront = true;
        }

        // 5.34 ���� ���� �ڵ��� (���� �� ���� �ִ� ��츸)s
        if (isFacingFront && playerX >= resetX)
        {
            animator.SetTrigger("Back");
            isFacingFront = false;
        }
    }
}
