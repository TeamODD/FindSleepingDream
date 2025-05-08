using UnityEngine;

public class PlayerStatus : MonoBehaviour, PlayerStatusInterface
{
    public int energy = 50; // ������ �� (Inspector���� ����)

    public int GetEnergy() => energy;

    public void UseEnergy(int amount)
    {
        energy = Mathf.Max(0, energy - amount); // 0 ������ �� ��������
    }
}
