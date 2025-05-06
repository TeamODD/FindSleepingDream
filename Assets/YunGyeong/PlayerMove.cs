using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    private Rigidbody2D rb;
    private InputAction moveAction, jumpAction, sprintAction, crouchAction; // Crouch 액션
    private bool isJumping;
    private Animator animator;
    public float speed = 3f;
    public float sprintMultiplier = 1.5f;
    public float jumpPower = 6f;
    private float originalScaleY; // 원래 Y 스케일
    public float crouchScaleY = 0.5f; // 쭈그려앉았을 때 Y 스케일
    private float originalSpeed; // 원래 속도
    public float crouchSpeedMultiplier = 0.7f; // 쭈그려앉았을 때 속도 배율

    public float energy = 100f;
    public float GetEnergy() => energy;


    [Header("조력자 관련")]
    [Tooltip("따라다니는 조력자 NPC")]
    public Transform helperNPC;

    [Tooltip("조력자와 멀어졌다고 판단하는 거리")]
    public float maxDistanceToHelper = 10f;

    [Header("스프린트 설정")]
    public float maxSprintTime = 5f;
    private float currentSprintTime = 0f;
    private bool canSprint = true; // 이제 실제로 사용함

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        sprintAction = InputSystem.actions.FindAction("Sprint");
        crouchAction = InputSystem.actions.FindAction("Crouch");

        originalScaleY = transform.localScale.y;
        originalSpeed = speed;

        jumpAction.performed += OnJumpPerformed;
    }

    private void OnDestroy()
    {
        jumpAction.performed -= OnJumpPerformed;
    }

    void OnJumpPerformed(InputAction.CallbackContext context)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.1f, LayerMask.GetMask("Ground"));
        if (hit.collider != null && !isJumping)
        {
            isJumping = true;
            rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        }
    }

    private void FixedUpdate()
    {
        var moveValue = moveAction.ReadValue<Vector2>().x;
        float currentSpeed = speed;

        if (sprintAction.IsPressed() && moveValue != 0 && energy > 0 && canSprint)
        {
            currentSpeed *= sprintMultiplier;
            UseEnergy(20f * Time.fixedDeltaTime);
            currentSprintTime += Time.fixedDeltaTime;

            if (currentSprintTime >= maxSprintTime)
            {
                currentSprintTime = maxSprintTime;
            }
        }
        else
        {
            currentSprintTime = Mathf.Max(0f, currentSprintTime - Time.fixedDeltaTime);
            energy = Mathf.Min(energy + 10f * Time.fixedDeltaTime, 100f);
        }

        rb.linearVelocity = new Vector2(moveValue * currentSpeed, rb.linearVelocity.y);

        if (moveValue > 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (moveValue < 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
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

        // 에너지로 canSprint 제어
        if (energy <= 0f)
        {
            canSprint = false;
        }
        else if (energy >= 100f)
        {
            canSprint = true;
        }

        if (crouchAction.IsPressed())
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchScaleY, transform.localScale.z);
            speed = originalSpeed * crouchSpeedMultiplier;
        }
        else
        {
            transform.localScale = new Vector3(transform.localScale.x, originalScaleY, transform.localScale.z);
            speed = originalSpeed;
        }

        if (helperNPC != null)
        {
            float distanceToHelper = Vector2.Distance(transform.position, helperNPC.position);
            if (distanceToHelper > maxDistanceToHelper)
            {
                Debug.LogWarning("조력자가 너무 멀리 떨어졌습니다!");
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("floor"))
        {
            isJumping = false;
        }
    }

    public void UseEnergy(float amount)
    {
        energy = Mathf.Max(0f, energy - amount);
    }
}
