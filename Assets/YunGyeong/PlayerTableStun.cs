using UnityEngine;

public class PlayerTableStun : MonoBehaviour
{
    private PlayerMove playerMove;
    private Animator animator;
    private bool isStunned = false;
    private float stunTimer = 0f;

    private float originalScaleY;

    private void Start()
    {
        playerMove = GetComponent<PlayerMove>();
        animator = GetComponent<Animator>();

        if (playerMove == null)
        {
            Debug.LogError("[PlayerTableStun] PlayerMove 컴포넌트 없음!");
        }

        originalScaleY = transform.localScale.y;
    }

    private void Update()
    {
        if (isStunned)
        {
            stunTimer -= Time.deltaTime;
            if (stunTimer <= 0f)
            {
                isStunned = false;
                Debug.Log("[PlayerTableStun] 스턴 해제");
            }
        }

        // 머리 위 테이블 감지 → 쭈그리기 강제 유지
        if (!IsCrouching())
        {
            Vector2 headPos = (Vector2)transform.position + Vector2.up * (originalScaleY / 2f + 0.05f);
            RaycastHit2D hit = Physics2D.Raycast(headPos, Vector2.up, 0.2f, LayerMask.GetMask("Table"));
            if (hit.collider != null)
            {
                animator.SetBool("IsCrouching", true);
                Debug.Log("[PlayerTableStun] 위에 테이블 있어서 강제 쭈그리기 유지");
            }
        }
    }

    public void TriggerStun(float duration)
    {
        if (isStunned) return;

        isStunned = true;
        stunTimer = duration;

        if (playerMove != null)
        {
            var rb = playerMove.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
                rb.angularVelocity = 0f;
            }

            if (animator != null)
            {
                animator.SetBool("Walk", false);
                animator.SetBool("IsSprinting", false);
                animator.SetTrigger("Stunned");
            }
        }

        Debug.Log("[PlayerTableStun] 스턴 발동: " + duration + "초");
    }

    public bool IsStunned()
    {
        return isStunned;
    }

    public bool IsCrouching()
    {
        return animator != null && animator.GetBool("IsCrouching");
    }
}
