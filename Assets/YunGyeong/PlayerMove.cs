using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    private Rigidbody2D rb;
    private InputAction moveAction, jumpAction;
    private bool isJumping;
    public float speed = 5f;
    public float jumpPower = 5f; 

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
    }

    private void FixedUpdate()
    {
        var moveValue = moveAction.ReadValue<Vector2>().x;

        // 걷는 방식 (즉시 속도 변경)
        rb.linearVelocity = new Vector2(moveValue * speed, rb.linearVelocity.y);

        if (moveValue > 0)
        {
            transform.localScale = new Vector3(0.8f, 0.8f, 1f);
        }
        else if (moveValue < 0)
        {
            transform.localScale = new Vector3(-0.8f, 0.8f, 1f);
        }

        if (jumpAction.IsPressed() && !isJumping)
        {
            isJumping = true;
            rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        }
    }

    private void Update()
    {
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("floor"))
        {
            isJumping = false;
        }
    }
}