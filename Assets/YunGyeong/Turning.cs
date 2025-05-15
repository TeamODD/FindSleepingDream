using UnityEngine;

public class Turning : MonoBehaviour
{
    public Transform playerTransform;
    public float triggerX = 60.03f; // 앞보기
    public float resetX = 64f;   // 뒤돌기
    private Animator animator;
    private bool isFacingFront = false; // 지금 앞 보고 있는 상태

    void Start()
    {
        animator = GetComponent<Animator>();
        if (playerTransform == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null) playerTransform = player.transform;
        }
    }

    void Update()
    {
        if (playerTransform == null) return;

        float playerX = playerTransform.position.x;

        // 0.66 넘을 때만 앞보기 (지금 뒤 보고 있는 경우만)
        if (!isFacingFront && playerX >= triggerX && playerX < 62f)
        {
            animator.SetTrigger("Front");
            isFacingFront = true;
        }

        // 5.34 넘을 때만 뒤돌기 (지금 앞 보고 있는 경우만)s
        if (isFacingFront && playerX >= resetX)
        {
            animator.SetTrigger("Back");
            isFacingFront = false;
        }
    }
}
