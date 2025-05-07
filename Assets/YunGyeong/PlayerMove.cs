using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic; // 인벤토리용

public class PlayerMove : MonoBehaviour
{
    private Rigidbody2D rb;
    private InputAction moveAction, jumpAction, sprintAction, crouchAction, interactAction;
    private bool isJumping;
    private Animator animator;
    public float speed = 3f;
    public float sprintMultiplier = 1.5f;
    public float jumpPower = 6f;
    private float originalScaleY;
    public float crouchScaleY = 0.5f;
    private float originalSpeed;
    public float crouchSpeedMultiplier = 0.7f;

    public float energy = 100f;
    public float GetEnergy() => energy;

    [Header("조력자 관련")]
    public Transform helperNPC;
    public float maxDistanceToHelper = 10f;

    [Header("스프린트 설정")]
    public float maxSprintTime = 5f;
    private float currentSprintTime = 0f;
    private bool canSprint = true;

    // =============================
    // ✅ 인벤토리 관련 변수 추가
    private HashSet<string> keyItems = new HashSet<string>();
    private int dreamShardCount = 0;
    private GameObject throwableItemPrefab = null;
    // =============================

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        sprintAction = InputSystem.actions.FindAction("Sprint");
        crouchAction = InputSystem.actions.FindAction("Crouch");
        interactAction = InputSystem.actions.FindAction("Interact");
        interactAction.performed += OnInteractPerformed;

        originalScaleY = transform.localScale.y;
        originalSpeed = speed;

        jumpAction.performed += OnJumpPerformed;
    }

    private void OnDestroy()
    {
        jumpAction.performed -= OnJumpPerformed;
        interactAction.performed -= OnInteractPerformed;
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

    void OnInteractPerformed(InputAction.CallbackContext context)
    {
        Debug.Log("C 키 눌림: 상호작용 시도 중");
        float interactRadius = 1.5f;

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, interactRadius);

        foreach (var hit in hits)
        {
            if (hit.CompareTag("item"))
            {
                var item = hit.GetComponent<IInteractable>();
                if (item != null)
                {
                    item.Interact();
                }
            }
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

    // =============================
    // ✅ 인벤토리용 공개 메서드들 추가
    public void CollectItem(string itemName)
    {
        keyItems.Add(itemName);
        Debug.Log($"[인벤토리] {itemName} 획득됨");
    }

    public bool HasItem(string itemName)
    {
        return keyItems.Contains(itemName);
    }

    public void AddDreamShard()
    {
        dreamShardCount++;
        Debug.Log($"[인벤토리] 꿈조각 수: {dreamShardCount}");
    }

    public int GetDreamShardCount()
    {
        return dreamShardCount;
    }

    public void SetThrowableItem(GameObject prefab)
    {
        throwableItemPrefab = prefab;
    }

    public bool HasThrowable()
    {
        return throwableItemPrefab != null;
    }

    public void Throw()
    {
        if (throwableItemPrefab != null)
        {
            GameObject go = Instantiate(throwableItemPrefab, transform.position + Vector3.right, Quaternion.identity);
            go.GetComponent<Rigidbody2D>()?.AddForce(Vector2.right * 5f, ForceMode2D.Impulse);
            throwableItemPrefab = null;
        }
    }
    // =============================
}
