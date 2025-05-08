using UnityEngine;

using UnityEngine;

public class Player_move : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    private Rigidbody2D rb;
    private bool isGrounded;

    public Transform groundCheck;
    public LayerMask groundLayer;

    private bool isHit = false;
    private float hitDuration = 1f;
    private float hitTimer = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!isHit)
        {
            MovePlayer();
            JumpPlayer();
        }
        else
        {
            hitTimer += Time.deltaTime;
            if (hitTimer >= hitDuration)
            {
                isHit = false;
               
            }
        }
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

    void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, 0.2f);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // 던진 장애물에만 반응
        if (collision.gameObject.GetComponent<Highlight>() && !isHit)
        {
            isHit = true;
            hitTimer = 0f;
            rb.linearVelocity = Vector2.zero;
       
        }
    }
}