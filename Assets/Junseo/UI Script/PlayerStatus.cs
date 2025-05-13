using UnityEngine;

public class PlayerStatus : MonoBehaviour, PlayerStatusInterface
{
    public float energy = 100f;

    private bool isDepleting = false;
    private bool isRecovering = false;
    private bool isWaitingForRecovery = false;
    private bool isRecoveryQueued = false;

    private float depletionRate = 100f / 5f;
    private float recoveryRate = 100f / 6f;

    private float recoveryDelay = 3f;
    private float recoveryDelayTimer = 0f;

    public bool CanSprint => !isRecovering && !isWaitingForRecovery;

    void Update()
    {
        // ✅ 스태미너 소모는 내부 상태에 따라 계속 지속
        if (isDepleting)
        {
            energy -= depletionRate * Time.deltaTime;

            if (energy <= 0f)
            {
                energy = 0f;
                isDepleting = false;
                StartRecoveryDelay(); // 0되면 자동 회복 예약
            }
        }

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

    public float GetEnergy() => energy;

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

    public void StartDepletion()
{
    if (energy > 0f)
    {
        isDepleting = true;

        // ✅ 회복 중이면 중단하고 지연 다시 시작
        if (isRecovering)
        {
            isRecovering = false;
            StartRecoveryDelay(); // 회복 → 지연 전환
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
        // ✅ 달리기 해제 시 정확히 끊음
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
}
