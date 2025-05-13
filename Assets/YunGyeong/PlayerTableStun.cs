using UnityEngine;
using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class PlayerTableStun : MonoBehaviour
{
    private PlayerMove playerMove;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private bool isForceCrouch = false; // ← 내부 상태 추적용
    private readonly Vector2 forcedCrouchSize = new Vector2(0.4167204f, 1.288672f);
    private readonly Vector2 forcedCrouchOffset = new Vector2(-0.05098605f, 0.6191691f);
    private float stunElapsed = 0f;
    private float currentStunDuration = 0f;
    private float stunCooldown = 1f; // 스턴과 스턴 사이의 쿨타임 (초)
    private float cooldownTimer = 0f; // 현재 남은 쿨타임
    private bool isTouchingTableHead = false;


    private bool isStunned = false;
    private float stunTimer = 0f;
    private Coroutine blinkCoroutine;

    private float originalScaleY;

    private void Start()
    {
        playerMove = GetComponent<PlayerMove>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        originalScaleY = transform.localScale.y;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("TableHead"))
        {
            isTouchingTableHead = true;

            if (cooldownTimer <= 0f)
            {
                TriggerStun(1f);
                cooldownTimer = stunCooldown; // 1초 쿨타임 시작
            }
        }
    }
    private void Update()
    {
        if (cooldownTimer > 0f)
            cooldownTimer -= Time.deltaTime;

        if (isStunned)
        {
            stunTimer -= Time.deltaTime;
            if (stunTimer <= 0f)
            {
                isStunned = false;
                animator.speed = 1f; // 애니메이션 재시작

                if (playerMove != null)
                {
                    var rb = playerMove.GetComponent<Rigidbody2D>();
                    if (rb != null)
                    {
                        rb.bodyType = RigidbodyType2D.Dynamic; // 🎯 다시 움직일 수 있게 복구
                    }
                }
                if (blinkCoroutine != null)
                    StopCoroutine(blinkCoroutine);
                spriteRenderer.color = Color.white;
            }
            if (stunElapsed < 1f && !isTouchingTableHead)
            {
                isForceCrouch = false; // 완전히 지나갔을 때만 일어날 수 있음
            }
            else
            {
                isForceCrouch = true; // 아직도 책상 아래로 판단해서 유지
            }

            // 스턴 중엔 무조건 쭈그리기 유지
            animator.SetBool("IsCrouching", true);
        }
        else if (isForceCrouch)
        {
            animator.SetBool("IsCrouching", true); // ⭐ 스턴 끝나도 강제쭈그리기 유지
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("TableHead"))
        {
            isTouchingTableHead = false;
        }
    }

    public void TriggerStun(float duration)
    {
        currentStunDuration = duration;
        stunElapsed = 0f;
        isStunned = true;
        stunTimer = duration;

        Debug.Log("[PlayerTableStun] 스턴 발동");

        if (playerMove != null)
        {
            var rb = playerMove.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
                rb.bodyType = RigidbodyType2D.Kinematic; // 스턴 중엔 밀리지 않게!
            }

        }

        if (animator != null)
        {
            animator.SetBool("Walk", false);
            animator.SetBool("IsSprinting", false);
            animator.SetBool("IsCrouching", true); // 강제 쭈그리기
            animator.speed = 0f;
        }

        if (spriteRenderer != null)
        {
            blinkCoroutine = StartCoroutine(BlinkDuringStun());
        }

    }


    //부딪히면 깜빡이기
    private IEnumerator BlinkDuringStun()
    {
        while (isStunned)
        {
            spriteRenderer.color = new Color(1f, 1f, 1f, 0.3f);
            yield return new WaitForSeconds(0.2f);
            spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
            yield return new WaitForSeconds(0.2f);
        }
    }

    public bool IsStunned()
    {
        return isStunned;
    }

    public bool IsCrouching()
    {
        return animator != null && animator.GetBool("IsCrouching");
    }
    public bool IsForceCrouching()
    {
        return isForceCrouch;
    }
}