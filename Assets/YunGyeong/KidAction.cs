using System.Xml.Linq;
using UnityEngine;

public class KidAction : MonoBehaviour
{
    public Animator animator;
    public Transform player;
    public GameObject PrisonDoor;
    public float checkDelay = 0.5f;
    public bool defaultLookingRight = true;
    public float stopDistance = 3f; //  캐릭터마다 거리 다르게!
    public float minY = -2.89f;

    private bool Walk = false;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        InvokeRepeating(nameof(PrisonDoorMethod), 0f, checkDelay);

        var rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.gravityScale = 0f; //  중력 꺼줌
        }

    }

    public void PrisonDoorMethod()
    {
        if (Walk) return;

        if (PrisonDoor == null)
        {
            Walk = true;
            animator.SetTrigger("Walk");
            Debug.Log($"[{name}] 탈출 시작!");
        }
    }

    void Update()
    {
        if (Walk)
        {
            Vector3 currentPos = transform.position;
            Vector3 targetPos = player.position;


            if (currentPos.y < minY)
            {
                currentPos.y = minY;
                transform.position = currentPos;
            }

            targetPos.z = currentPos.z;

            float distance = Vector3.Distance(currentPos, player.position);
            bool walking = distance > stopDistance;

            animator.SetBool("Walk", walking);

            if (walking)
                transform.position = Vector3.MoveTowards(currentPos, targetPos, 2f * Time.deltaTime);

            if (spriteRenderer != null)
                spriteRenderer.flipX = (player.position.x < transform.position.x) != defaultLookingRight;
        }
    }
}
