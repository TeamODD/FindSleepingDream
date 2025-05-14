using UnityEngine;

public class PlayerStatus : MonoBehaviour, PlayerStatusInterface
{
    public float energy = 100f;

    private bool isDepleting = false;
    private bool isRecovering = false;
    private bool isWaitingForRecovery = false;
    private bool isRecoveryQueued = false;

    private float depletionRate = 100f / 5f;  // 5초 만에 100 소모
    private float recoveryRate = 100f / 6f;   // 6초 만에 100 회복

    private float recoveryDelay = 1.5f;
    private float recoveryDelayTimer = 0f;

    public float GetEnergy() => energy;

    // ✅ 에너지 1 이상이면 언제든 달리기 가능
    public bool CanSprint => energy > 0f;

    void Update()
    {
        // ✅ 스태미너 소모 처리
        if (isDepleting)
        {
            energy -= depletionRate * Time.deltaTime;

            if (energy <= 0f)
            {
                energy = 0f;
                isDepleting = false;
                StartRecoveryDelay();
            }
        }

        // ✅ 회복 지연 처리
        if (isWaitingForRecovery)
        {
            recoveryDelayTimer -= Time.deltaTime;

            if (recoveryDelayTimer <= 0f)
            {
                isWaitingForRecovery = false;

                if (isRecoveryQueued)
                {
                    StartRecovery();
                    isRecoveryQueued = false;
                }
            }
        }

        // ✅ 회복 처리
        if (isRecovering)
        {
            energy += recoveryRate * Time.deltaTime;

            if (energy >= 100f)
            {
                energy = 100f;
                isRecovering = false;
            }
        }
    }

    public void StartDepletion()
    {
        if (energy > 0f)
        {
            isDepleting = true;

            // ✅ 회복 중이면 중단 + 회복 지연 재시작
            if (isRecovering)
            {
                isRecovering = false;
                StartRecoveryDelay();
            }

            // ✅ 회복 지연 중이면 타이머 리셋
            if (isWaitingForRecovery)
            {
                recoveryDelayTimer = recoveryDelay;
            }
        }
    }

    public void StopDepletion()
    {
        if (isDepleting)
        {
            isDepleting = false;
            StartRecoveryDelay();
        }
    }

    private void StartRecoveryDelay()
    {
        if (!isRecovering)
        {
            isWaitingForRecovery = true;
            isRecoveryQueued = true;
            recoveryDelayTimer = recoveryDelay;
        }
    }

    private void StartRecovery()
    {
        isRecovering = true;
    }

    // ✅ 외부에서 강제 사용 시 (사용은 안 하고 있음)
    public void UseEnergy(float amount)
    {
        energy -= amount;
        if (energy <= 0f)
        {
            energy = 0f;
            isDepleting = false;
            StartRecoveryDelay();
        }
    }
}
