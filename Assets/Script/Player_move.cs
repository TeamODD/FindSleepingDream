using UnityEngine;

public class Player_move : MonoBehaviour
{
    public float moveSpeed = 5f;  // �̵� �ӵ�
    public float jumpForce = 10f; // ���� ��
    private Rigidbody2D rb;
    private bool isGrounded;  // �ٴڿ� �ִ��� Ȯ���ϴ� ����

    public Transform groundCheck;  // �ٴ� üũ�� ���� ��ġ
    public LayerMask groundLayer;  // �ٴ��� ���̾�

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        MovePlayer();
        JumpPlayer();
    }

    void MovePlayer()
    {
        // �̵� (WASD�� ������)
        float moveInput = Input.GetAxis("Horizontal");  // A/D �Ǵ� ����/������ ȭ��ǥ
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);  // ���� �̵�, ���� �ӵ��� �״�� ����
    }

    void JumpPlayer()
    {
        // �ٴڿ� ���� ���� ���� ����
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            // �����̽��ٸ� ������ ����
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);  // ���� �ӵ��� �״�� �ΰ� ����
        }
    }

    // �����Ϳ��� �ٴ� üũ�� �ð�ȭ�ϱ� ���� �Լ�
    void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, 0.2f);
        }
    }
}
