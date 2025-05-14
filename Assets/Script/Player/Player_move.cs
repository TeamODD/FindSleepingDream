using UnityEngine;

public class Player_move : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    private Rigidbody2D rb;
    private bool isGrounded;

    public Transform groundCheck;
    public LayerMask groundLayer;



    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (isStunned)
        {
            stunTimer -= Time.deltaTime;
            if (stunTimer <= 0f)
                isStunned = false;

            return; // 움직임 막기
        }
        MovePlayer();
        JumpPlayer();
    }

        void MovePlayer()
        {
            float moveInput = Input.GetAxis("Horizontal");
            rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
        }

        void JumpPlayer()
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

            if (isGrounded && Input.GetKeyDown(KeyCode.Space))
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            }
        }
    

    private bool isStunned = false;
    private float stunDuration = 1.5f;
    private float stunTimer;

    
    public void Stun(float duration)
    {
        isStunned = true;
        stunTimer = duration;
       
    }

}