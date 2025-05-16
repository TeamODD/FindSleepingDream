using UnityEngine;
using UnityEngine.UI;

public class EnergyBarUI : MonoBehaviour
{
    public Image barFillImage;                  // ä������ �̹���
    public MonoBehaviour playerStatusProvider;  // ������ ���� �����ϴ� ������Ʈ

    private PlayerStatusInterface playerStatus;

    void Start()
    {
        playerStatus = playerStatusProvider.GetComponent<PlayerStatus>();

        if (playerStatus == null)
        {
            Debug.LogError("[EnergyBarUI] PlayerStatusInterface ĳ���� ������!");
        }
        else
        {
            Debug.Log("[EnergyBarUI] ĳ���� ����, ������: " + playerStatus.GetEnergy());
        }
    }

    void Update()
    {
        if (playerStatus != null)
        {
            float energy = playerStatus.GetEnergy();             // ������ �� �޾ƿ��� (��: 75)
            float ratio = Mathf.Clamp01(energy / 100f);          // 0~1 ���̷� ��ȯ
            barFillImage.fillAmount = ratio;                     // �̹��� �� ä���
        }
    }
}
