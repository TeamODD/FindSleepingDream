using UnityEngine;

public class PlayerStatus : MonoBehaviour, PlayerStatusInterface
{
    public int energy = 50; // 에너지 값 (Inspector에서 조절)

    public int GetEnergy() => energy;

    public void UseEnergy(int amount)
    {
        energy = Mathf.Max(0, energy - amount); // 0 밑으로 안 떨어지게
    }
}
