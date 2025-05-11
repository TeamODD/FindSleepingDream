using UnityEngine;

public class PlayerTableStun : MonoBehaviour
{
    private PlayerMove playerMove;
    private bool isStunned = false;
    private float stunTimer = 0f;

    private void Start()
    {
        playerMove = GetComponent<PlayerMove>();
        if (playerMove == null)
        {
            Debug.LogError("[PlayerTableStun] PlayerMove");
        }
    }

    private void Update()
    {
        if (!isStunned) return;

        stunTimer -= Time.deltaTime;
        if (stunTimer <= 0f)
        {
            isStunned = false;
            Debug.Log("[PlayerTableStun]");
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

            var animator = playerMove.GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetBool("Walk", false);
                animator.SetBool("IsSprinting", false);
                animator.SetTrigger("Stunned"); // Ʈ���� �ִϸ��̼� ���� ����
            }
        }

        Debug.Log("[PlayerTableStun] " + duration + "��");
    }

    public bool IsStunned()
    {
        return isStunned;
    }

    public bool IsCrouching()
    {
        // Y ������ ������� crouch ���� ����
        return Mathf.Abs(transform.localScale.y - playerMove.crouchScaleY) < 0.01f;
    }
}
