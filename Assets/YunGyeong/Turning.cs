using UnityEngine;

public class Turning : MonoBehaviour
{
    public Transform playerTransform;
    public float triggerX = 0.66f; // 플레이어 X 좌표가 이 이상일 때 발동
    public float resetX = 5.34f;
    private Animator animator;
    private bool triggered = false;

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

        if (!triggered && playerX >= triggerX)
        {
            animator.SetTrigger("Turn");
            triggered = true;
        }

        // 리셋 조건 (다시 아래로 내려가면 triggered = false)
        if (triggered && playerX < resetX)
        {
            triggered = false;
        }
    }
}
