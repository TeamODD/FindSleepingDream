using UnityEngine;

public class Turning : MonoBehaviour
{
    public Transform playerTransform;
    public float triggerX = 0.66f; // �÷��̾� X ��ǥ�� �� �̻��� �� �ߵ�
    public float resetX = 5.34f;
    private Animator animator;
    private bool triggered = false;

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

        if (!triggered && playerX >= triggerX)
        {
            animator.SetTrigger("Turn");
            triggered = true;
        }

        // ���� ���� (�ٽ� �Ʒ��� �������� triggered = false)
        if (triggered && playerX < resetX)
        {
            triggered = false;
        }
    }
}
