using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic; // 인벤토리용
using System.Collections;

public class PlayerMove : MonoBehaviour
{
    private Rigidbody2D rb;
    private InputAction moveAction, jumpAction, sprintAction, crouchAction, interactAction;
    private bool isJumping;
    private bool isCrouching;
    private Animator animator;
    public float speed = 3f;
    public float sprintMultiplier = 1.5f;
    public float jumpPower = 6f;
    private float originalSpeed;
    public InventoryManager inventoryManager;
    private PlayerTableStun stunController;
    public LayerMask RayObject;
    public LayerMask FloorRay;

    // 쭈그렸을 때 박스콜라이더 축소로 책상 지나갈 수 있도록 선언
    private BoxCollider2D boxCollider;
    private Vector2 originalSize;
    private Vector2 crouchSize;
    private Vector2 originalOffset;
    private Vector2 crouchOffset;

    public float crouchSpeedMultiplier = 0.7f;

    [Header("조력자 관련")]
    public Transform helperNPC;
    public float maxDistanceToHelper = 10f;

    [Header("스프린트 설정")]
    public float maxSprintTime = 5f;
    private float currentSprintTime = 0f;
    private PlayerStatus status;

    // =============================
    // ✅ 인벤토리 관련 변수 추가
    private HashSet<string> keyItems = new HashSet<string>();
    private int dreamShardCount = 0;
    private GameObject throwableItemPrefab = null;

    // =============================

private bool isInputBlocked = false;

public void SetInputBlocked(bool blocked)
{
    isInputBlocked = blocked;
    Debug.Log($"[Player] 입력 전체 차단 상태: {blocked}");
}

public void BlockInputForSeconds(float duration)
{
    StartCoroutine(BlockInputTemporarily(duration));
}

    private IEnumerator BlockInputTemporarily(float seconds)
    {
        isInputBlocked = true;
        Debug.Log($"⏱️ {seconds}초간 입력 전체 차단");
        yield return new WaitForSeconds(seconds);
        isInputBlocked = false;
        Debug.Log("✅ 입력 다시 가능");
    }

private bool isMoveBlocked = false;

public void SetMoveBlocked(bool blocked)
{
    isMoveBlocked = blocked;
    Debug.Log($"[Player] 이동 차단 상태: {blocked}");
}

public void BlockMoveForSeconds(float duration)
{
    StartCoroutine(BlockMoveTemporarily(duration));
}

private IEnumerator BlockMoveTemporarily(float seconds)
{
    isMoveBlocked = true;
    Debug.Log($"⏱️ {seconds}초간 이동만 차단");
    yield return new WaitForSeconds(seconds);
    isMoveBlocked = false;
    Debug.Log("✅ 이동 다시 가능");
}




    private void Start()
    {

        status = GetComponent<PlayerStatus>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        sprintAction = InputSystem.actions.FindAction("Sprint");
        crouchAction = InputSystem.actions.FindAction("Crouch");
        interactAction = InputSystem.actions.FindAction("Interact");
        interactAction.performed += OnInteractPerformed;


        originalSpeed = speed;

        jumpAction.performed += OnJumpPerformed;
        if (status == null)
        {
            Debug.LogError("[PlayerMove] PlayerStatus 컴포넌트를 찾을 수 없습니다.");
        }

        //쭈그렸을 때 박스 콜라이더 축소
        boxCollider = GetComponent<BoxCollider2D>();
        if (boxCollider != null)
        {
            originalSize = boxCollider.size;
            originalOffset = boxCollider.offset;

            // 원하는 쭈그린 상태의 사이즈와 오프셋으로 설정!
            crouchSize = new Vector2(originalSize.x, originalSize.y * 0.5f);
            // 높이 절반으로
            crouchOffset = new Vector2(originalOffset.x, originalOffset.y - (originalSize.y * 0.25f)); // 아래로 약간 내림
        }
        //스턴 되었을 때 모든 움직임 차단


        stunController = GetComponent<PlayerTableStun>();

    }

    private void OnDestroy()
    {
        jumpAction.performed -= OnJumpPerformed;
        interactAction.performed -= OnInteractPerformed;
    }

    void OnJumpPerformed(InputAction.CallbackContext context)
    {
        if (isInputBlocked || isMoveBlocked) return;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.3f, LayerMask.GetMask("Ground"));
        if (hit.collider == null)
    Debug.LogWarning(">> 바닥에 안 닿음!");

        if (hit.collider != null && !isJumping && !isCrouching)
        {
            Debug.Log(">> 점프 입력됨");
            isJumping = true;
            rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        }
    }

    private Transform FindNearestMonster()
    {
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
        Transform nearest = null;
        float minDistance = Mathf.Infinity;

        foreach (GameObject monster in monsters)
        {
            float dist = Vector2.Distance(transform.position, monster.transform.position);
            if (dist < minDistance)
            {
                minDistance = dist;
                nearest = monster.transform;
            }
        }

        return nearest;
    }

    void OnInteractPerformed(InputAction.CallbackContext context)
    {
        Debug.Log("C 키 눌림: 상호작용 시도 중");
        float interactRadius = 1.5f; // 아이템 인식 범위

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

            if (hit.CompareTag("AttackItem"))
            {
                var attackItem = hit.GetComponent<AttackItem>();
                if (attackItem != null && !attackItem.IsUsed)
                {
                    attackItem.Activate();
                }
            }
        }
    }

    private void FixedUpdate()
    {

    if (isInputBlocked || isMoveBlocked)
    {
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y); // 이동 정지
        return;
    }

        float moveValue = moveAction.ReadValue<Vector2>().x;
        bool isMoving = Mathf.Abs(moveValue) > 0.01f;
        bool wantsToSprint = sprintAction.IsPressed();
        bool canSprint = status != null && status.CanSprint;
        float currentSpeed = speed;
        
   
        // ✅ 실제 달리기 조건
        bool shouldSprint = isMoving && wantsToSprint && !isCrouching && canSprint;

        if (shouldSprint)
        {
            currentSpeed = speed * sprintMultiplier;
            status.StartDepletion();
            Debug.Log("달리기가능");
        }
        else
        {
            // ✅ 방향키를 떼면, 즉 "달리기 입력 중단"이면 무조건 StopDepletion 호출
            if (!isMoving || !wantsToSprint)
            {
                status.StopDepletion();
            }


            //스턴시 행동 멈춤
            if (stunController != null && stunController.IsStunned())
            {
                rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);  // 수평 이동 정지
                return;
            }


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
    }


    private void Update()
    {
        if (isInputBlocked)
    {
        animator.SetBool("Walk", false);
        animator.SetBool("IsSprinting", false);
        animator.SetBool("IsCrouching", false);
        return;
    }

    if (isMoveBlocked)
    {
        // 이동만 막는 상태: 이동 관련 애니메이션도 끔
        animator.SetBool("Walk", false);
        animator.SetBool("IsSprinting", false);
        animator.SetBool("IsCrouching", false);
        return;
    }
        isCrouching = crouchAction.IsPressed() && !isJumping;

        bool isWalking = moveAction.IsPressed();
        bool wantsToSprint = sprintAction.IsPressed();
        bool canSprint = status != null && status.CanSprint;

        bool isSprinting = wantsToSprint && isWalking && !isCrouching && canSprint;

        // 애니메이션 상태 설정
        animator.SetBool("IsSprinting", isSprinting);
        animator.SetBool("Walk", isWalking && !isCrouching && !isSprinting);
        animator.SetBool("IsCrouching", isCrouching);

        
        speed = isCrouching ? originalSpeed * crouchSpeedMultiplier : originalSpeed;

            // 조력자 거리 경고
            if (helperNPC != null)
            {
                Debug.Log("NPC");
                float distanceToHelper = Vector2.Distance(transform.position, helperNPC.position);
                if (distanceToHelper > maxDistanceToHelper)
                {
                    Debug.LogWarning("조력자가 너무 멀리 떨어졌습니다!");
                }
        }

        bool isForceCrouching = false;


        if (stunController != null && stunController.IsForceCrouching())  // ✅ 요거 추가로 만들어야 함!
        {
            isForceCrouching = true;
        }

        // ▶ 콜라이더 강제 조절

        if (isCrouching || isForceCrouching)
        {   
            speed = originalSpeed * crouchSpeedMultiplier;

            boxCollider.size = crouchSize;
            boxCollider.offset = crouchOffset;
            animator.SetBool("IsCrouching", true);
            Debug.Log("쭈그린 상태에서의 콜라이더");

            rb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;

        }

        else
        {
            boxCollider.size = originalSize;
            boxCollider.offset = originalOffset;
            animator.SetBool("IsCrouching", false);

            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
        

        if (Physics2D.Raycast(transform.position, Vector2.up, 5f, RayObject))
        {
            Debug.Log("hit");
            animator.SetBool("IsCrouching", true);
            rb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;
        }

        if (Physics2D.Raycast(transform.position, Vector2.down, 0.1f, FloorRay))
        {   
            isJumping = false;

        }
        else
        {
            isJumping = true;
        }
    }
    public void CollectItem(string itemName)
    {
        keyItems.Add(itemName);
        Debug.Log($"[인벤토리] {itemName} 획득됨");
        inventoryManager?.AddItem(itemName); //  for UI
    }

    public bool HasItem(string itemName)
    {
        return keyItems.Contains(itemName);
    }

    public void AddDreamShard()
    {
        dreamShardCount++;
        Debug.Log($"[인벤토리] 꿈조각 수: {dreamShardCount}");

        inventoryManager?.AddItem("Star");
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

    // =============================
    // ✅ 기물에 맞았을 때 죽는 기능
    private bool isDead = false;

    public void Die()
    {
        if (!isDead)
        isDead = true;

        if (rb != null)
            rb.simulated = false;

        animator.SetBool("Walk", false);

        Debug.Log("[Player] 플레이어가 기물에 맞아 죽었습니다.");
    }
    // =============================


}