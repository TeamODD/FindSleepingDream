using UnityEngine;

public class ItemHint : MonoBehaviour
{
    public GameObject hintImage;  // 표시할 힌트 이미지
    public float verticalOffset = 1.2f;
    public bool faceCamera = true;

    [Header("자동 위치 조정 설정")]
    public bool useAutoPosition = true; // ✅ 자동 위치 조정 여부
    public Vector3 customOffset = new Vector3(1f, 1.2f, 1f); // ✅ 수동 조정용 오프셋

    private void Start()
    {
        if (hintImage == null)
        {
            hintImage = transform.Find("HintImage")?.gameObject;
            Debug.Log("[HintTrigger] 자동으로 HintImage 연결 시도");
        }

        if (hintImage == null)
            Debug.LogWarning("[HintTrigger] hintImage 연결 실패!");
        else
            hintImage.SetActive(false);
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
            if (useAutoPosition)
            {
                // ✅ 자동 위치 조정: verticalOffset 또는 customOffset 사용
                Vector3 offset = customOffset; // 필요시 verticalOffset만 쓰는 걸로 바꿔도 됨
                hintImage.transform.position = transform.position + offset;
            }
            // ✅ faceCamera 설정 시 회전 고정
            if (faceCamera)
                hintImage.transform.rotation = Quaternion.identity;
        }
    }
}
