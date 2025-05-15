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
    private float originalSpeed;
    public InventoryManager inventoryManager;
    private PlayerTableStun stunController;



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


    // 아이템 먹을때 오디오 설정
    public AudioSource audioStar; // 별 먹을떄 소리
    public AudioSource audioKey; // 열쇠 먹을 때 소리
    public AudioSource audioButton; // 감옥 버튼
    public AudioSource audioAttack; // 타임어택


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

        originalScaleY = transform.localScale.y;
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
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.1f, LayerMask.GetMask("Ground"));
        if (hit.collider == null)
    Debug.LogWarning(">> 바닥에 안 닿음!");

        if (hit.collider != null && !isJumping)
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
                    if (!audioStar.isPlaying)  // 여기 오디오
                        audioStar.Play();
                }
            }

            if (hit.CompareTag("Key"))
            {
                if (!audioKey.isPlaying)
                    audioKey.Play();

            }

            if (hit.CompareTag("Button"))
            {
                if (!audioButton.isPlaying)
                {
                    audioButton.Play();
                    audioAttack.Play();
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
    float moveValue = moveAction.ReadValue<Vector2>().x;
    bool isMoving = Mathf.Abs(moveValue) > 0.01f;
    bool isCrouching = crouchAction.IsPressed();
    bool wantsToSprint = sprintAction.IsPressed();
    bool canSprint = status != null && status.CanSprint;

    float currentSpeed = speed;

    // ✅ 실제 달리기 조건
    bool shouldSprint = isMoving && wantsToSprint && !isCrouching && canSprint;

    if (shouldSprint)
    {
        currentSpeed = speed * sprintMultiplier;
        status.StartDepletion();
    }
    else
    {
        // ✅ 방향키를 떼면, 즉 "달리기 입력 중단"이면 무조건 StopDepletion 호출
        if (!isMoving || !wantsToSprint)
        {
            status.StopDepletion();
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

        //스턴시 행동 멈춤
        if (stunController != null && stunController.IsStunned())
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);  // 수평 이동 정지
            return;
        }

    }

rb.linearVelocity = new Vector2(moveValue * currentSpeed, rb.linearVelocity.y);

    if (moveValue > 0)
        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    else if (moveValue < 0)
        transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
}


    private void Update()
{
    bool isCrouching = crouchAction.IsPressed();
    bool isWalking = moveAction.IsPressed();
    bool wantsToSprint = sprintAction.IsPressed();
    bool canSprint = status != null && status.CanSprint;

    bool isSprinting = wantsToSprint && isWalking && !isCrouching && canSprint;

    // 애니메이션 상태 설정
    animator.SetBool("IsSprinting", isSprinting);
    animator.SetBool("Walk", isWalking && !isCrouching && !isSprinting);
    animator.SetBool("IsCrouching", isCrouching);

    // 디버그 및 속도 조정
    Debug.Log("Move Value: " + moveAction.ReadValue<Vector2>());

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

        var tableStun = GetComponent<PlayerTableStun>();
        if (tableStun != null && tableStun.IsForceCrouching())  // ✅ 요거 추가로 만들어야 함!
        {
            isForceCrouching = true;
        }
            // ▶ 콜라이더 강제 조절
            if (isForceCrouching)
            {
                boxCollider.size = new Vector2(0.4167204f, 1.288672f);
                boxCollider.offset = new Vector2(-0.05098605f, 0.6191691f);
                animator.SetBool("IsCrouching", true); // 애니메이션 쭈그리기 유지

                rb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;

            }
            else if (isCrouching)
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

            }
    
}


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("floor"))
        {
            isJumping = false;
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
        if (isDead) return;
        isDead = true;

        if (rb != null)
            rb.simulated = false;

        animator.SetBool("Walk", false);

        Debug.Log("[Player] 플레이어가 기물에 맞아 죽었습니다.");
    }
    // =============================


}