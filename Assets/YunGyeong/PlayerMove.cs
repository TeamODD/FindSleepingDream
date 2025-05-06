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
    private PlayerStatus status; // 에너지 상태 참조



    [Header("조력자 관련")]
    [Tooltip("따라다니는 조력자 NPC")]
    public Transform helperNPC; // 이 변수가 없어서 오류 발생

    [Tooltip("조력자와 멀어졌다고 판단하는 거리")]
    public float maxDistanceToHelper = 10f; // 이 변수가 없어서 오류 발생

    private void Awake() // Start 대신 Awake에서 액션에 이벤트 핸들러 등록
    {
        status = GetComponent<PlayerStatus>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        sprintAction = InputSystem.actions.FindAction("Sprint");
        crouchAction = InputSystem.actions.FindAction("Crouch"); // Crouch 액션 찾기

        originalScaleY = transform.localScale.y; // 시작 시 Y 스케일 저장
        originalSpeed = speed; // 시작 시 속도 저장

        // 점프 액션의 triggered 이벤트에 점프 함수 연결
        jumpAction.performed += OnJumpPerformed;
    }

    private void OnDestroy() // 오브젝트가 파괴될 때 이벤트 핸들러 해제
    {
        jumpAction.performed -= OnJumpPerformed;
    }

    void OnJumpPerformed(InputAction.CallbackContext context)
    {
        // 땅에 닿아있을 때만 점프 가능
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

        if (sprintAction.IsPressed() && moveValue != 0)
        {
            currentSpeed *= sprintMultiplier;

            // 에너지 소모 추가
            if (status != null)
            {
                status.UseEnergy(1); // 1씩 소모 (속도 조절은 나중에 가능)
            }
        }




        if (sprintAction.IsPressed() && moveValue != 0)
        {
            currentSpeed *= sprintMultiplier;
        }

        if (crouchAction.IsPressed())
        {
            currentSpeed *= crouchSpeedMultiplier;
        }

        rb.linearVelocity = new Vector2(moveValue * currentSpeed, rb.linearVelocity.y);

        // 좌우 반전 로직 (기존 코드 유지)
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

        // 쭈그려앉기 시 Y 스케일 및 속도 조절
        if (crouchAction.IsPressed())
        {
            // Y 스케일 줄여서 쭈그려앉은 것처럼 보이게 함
            transform.localScale = new Vector3(transform.localScale.x, crouchScaleY, transform.localScale.z);
            // 이동 속도 감소 (Inspector 값 사용)
            speed = originalSpeed * crouchSpeedMultiplier;
        }
        else
        {
            // Z 키에서 손을 떼면 원래 Y 스케일과 속도로 복원
            transform.localScale = new Vector3(transform.localScale.x, originalScaleY, transform.localScale.z);
            speed = originalSpeed;
        }

        // [기존에 오류가 발생했던 부분 수정]
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
}