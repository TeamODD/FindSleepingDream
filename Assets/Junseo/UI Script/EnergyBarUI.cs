using UnityEngine;
using UnityEngine.UI;

public class EnergyBarUI : MonoBehaviour
{
    public Image barFillImage;                  // 채워지는 이미지
    public MonoBehaviour playerStatusProvider;  // 에너지 값을 제공하는 오브젝트

    private PlayerStatusInterface playerStatus;

    void Start()
    {
        playerStatus = playerStatusProvider as PlayerStatusInterface;
    }

    void Update()
    {
        if (playerStatus != null)
        {
            float energy = playerStatus.GetEnergy();             // 에너지 값 받아오기 (예: 75)
            float ratio = Mathf.Clamp01(energy / 100f);          // 0~1 사이로 변환
            barFillImage.fillAmount = ratio;                     // 이미지 바 채우기
        }
    }
}
