using UnityEngine;

public class ItemHint : MonoBehaviour
{
    public GameObject hintImage;  // ��� �̹��� ������Ʈ
    public float verticalOffset = 1.2f;
    public bool faceCamera = true;

    private void Start()
    {
        // �ڵ� ���� �õ� (hintImage �̿��� ��)
        if (hintImage == null)
        {
            hintImage = transform.Find("HintImage")?.gameObject;
            Debug.Log("[HintTrigger] �ڵ����� HintImage ���� �õ�");
        }

        // ���� ���� ���
        if (hintImage == null)
            Debug.LogWarning("[HintTrigger] hintImage ���� ����!");
        else
            hintImage.SetActive(false);  // ���� �� ����
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && hintImage != null)
        {
            hintImage.SetActive(true);
            Debug.Log("[Hint] ��Ʈ ǥ�õ�");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && hintImage != null)
        {
            hintImage.SetActive(false);
            Debug.Log("[Hint] ��Ʈ ����");
        }
    }

    private void Update()
    {
        if (hintImage != null)
        {
            // ������ ��ġ �ٷ� ���� �̹��� ��ġ ����
            Vector3 offset = new Vector3(0, verticalOffset, 0);
            hintImage.transform.position = transform.position + offset;

            // ȸ�� ���� (ī�޶� ����)
            if (faceCamera)
                hintImage.transform.rotation = Quaternion.identity;
        }
    }
}
