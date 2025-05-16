using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    public float parallaxFactor = 0.2f;
    public Transform zoneAnchor;
    public int visibleZone = 0;  // 이 배경이 보일 zone index

    private Transform cam;

    void Start()
    {
        cam = Camera.main.transform;
        CheckVisibility();
    }

    void LateUpdate()
{

    CheckVisibility();

    if (!gameObject.activeSelf || zoneAnchor == null) return;

    Vector3 camDelta = cam.position - zoneAnchor.position;
    Vector3 newPos = zoneAnchor.position + camDelta * parallaxFactor;

    // ✅ Z값은 현재 배경의 Z 그대로 유지
    newPos.z = transform.position.z;

    transform.position = newPos;
}


    void CheckVisibility()
{
    if (!CameraManagerExists()) return;

    bool isVisible = (CameraManager.CurrentZone == visibleZone);

    if (TryGetComponent<SpriteRenderer>(out var sr))
    {
        sr.enabled = isVisible;
    }
}


    bool CameraManagerExists()
    {
        return typeof(CameraManager).GetProperty("CurrentZone") != null;
    }
}
