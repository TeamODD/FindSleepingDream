using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    private Rigidbody2D rb;
    private InputAction moveAction, jumpAction, sprintAction; // 스프린트 액션 추가
    private Animator animator;
    private bool isJumping;
    public float speed = 5f;
    public float sprintMultiplier = 2f; // 스프린트 속도 배율
    public float jumpPower = 5f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        sprintAction = InputSystem.actions.FindAction("Sprint"); // 스프린트 액션 찾기
    }

    private void FixedUpdate()
    {
        var moveValue = moveAction.ReadValue<Vector2>().x;
        float currentSpeed = speed;

        // 스프린트 키가 눌려 있으면 속도 증가
        if (sprintAction.IsPressed())
        {
            currentSpeed *= sprintMultiplier;
        }

        // 걷는 방식 (즉시 속도 변경)
        rb.linearVelocity = new Vector2(moveValue * currentSpeed, rb.linearVelocity.y);

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
        bool isWalking = moveAction.IsPressed();
        animator.SetBool("Walk", isWalking);
        animator.SetBool("IsSprinting", sprintAction.IsPressed() && isWalking); // 달리기 상태 업데이트
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("floor"))
        {
            isJumping = false;
        }
    }
}