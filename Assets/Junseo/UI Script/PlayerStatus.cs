using UnityEngine;

public class PlayerStatus : MonoBehaviour, PlayerStatusInterface
{
    public float energy = 100f;

    public float GetEnergy() => energy;

    public void UseEnergy(float amount)
    {
        energy = Mathf.Max(0f, energy - amount);
        Debug.Log("Energy: " + energy); // ← 로그 확인용!
    }
}
