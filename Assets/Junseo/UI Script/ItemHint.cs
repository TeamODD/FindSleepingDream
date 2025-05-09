using UnityEngine;

public class ItemHint : MonoBehaviour
{
    public GameObject hintImage;  // 띄울 이미지 오브젝트
    public float verticalOffset = 1.2f;
    public bool faceCamera = true;

    private void Start()
    {
        // 자동 연결 시도 (hintImage 미연결 시)
        if (hintImage == null)
        {
            hintImage = transform.Find("HintImage")?.gameObject;
            Debug.Log("[HintTrigger] 자동으로 HintImage 연결 시도");
        }

        // 연결 실패 경고
        if (hintImage == null)
            Debug.LogWarning("[HintTrigger] hintImage 연결 실패!");
        else
            hintImage.SetActive(false);  // 시작 시 숨김
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && hintImage != null)
        {
            hintImage.SetActive(true);
            Debug.Log("[Hint] 힌트 표시됨");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && hintImage != null)
        {
            hintImage.SetActive(false);
            Debug.Log("[Hint] 힌트 숨김");
        }
    }

    private void Update()
    {
        if (hintImage != null)
        {
            // 아이템 위치 바로 위로 이미지 위치 고정
            Vector3 offset = new Vector3(0, verticalOffset, 0);
            hintImage.transform.position = transform.position + offset;

            // 회전 고정 (카메라 정면)
            if (faceCamera)
                hintImage.transform.rotation = Quaternion.identity;
        }
    }
}
