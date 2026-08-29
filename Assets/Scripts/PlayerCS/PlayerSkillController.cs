using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSkillController : MonoBehaviour
{
    [Header("Slash Skill Settings")]
    [SerializeField] private GameObject slashWavePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float slashSkillCooldown = 1f;

    [Header("Whirlwind Skill Settings")]
    [SerializeField] private float whirlwindSkillCooldown = 2f;

    [Header("Dash Skill Settings")]
    [SerializeField] private float DashSkillCooldown = 2f;

    [Header("Animation")]
    [SerializeField] private Animator animator;
    [SerializeField] private string slashSkillTriggerName = "SlashSkill";
    [SerializeField] private string whirlwindSkillTriggerName = "WhirlwindSkill";
    [SerializeField] private string DashSkillTriggerName = "DashSkill";

    [Header("Reference")]
    [SerializeField] private PlayerInputHandler actionState;

    private float lastSlashSkillTime = -999f;
    private float lastWhirlwindSkillTime = -999f;
    private float lastDashSkillTime = -999f;

    private bool isUsingSkill;

    private void Awake()
    {
        if (animator == null)
            animator = GetComponent<Animator>();

        if (actionState == null)
            actionState = GetComponent<PlayerInputHandler>();
    }

    // -----------------------------
    // Slash Skill Input
    // -----------------------------
    public void OnSlashSkill(InputAction.CallbackContext context)
    {
        if (!context.started)
            return;

        if (isUsingSkill)
            return;

        if (Time.time < lastSlashSkillTime + slashSkillCooldown)
            return;

        if (actionState != null && !actionState.TryEnterSkill())
            return;

        UseSlashSkill();
    }

    private void UseSlashSkill()
    {
        lastSlashSkillTime = Time.time;
        isUsingSkill = true;

        if (animator != null)
        {
            animator.ResetTrigger("Attack1");
            animator.ResetTrigger("Attack2");
            animator.ResetTrigger(whirlwindSkillTriggerName);
            animator.SetTrigger(slashSkillTriggerName);
        }
        else
        {
            SpawnSlashWave();
            EndSkill();
        }
    }

    public void SpawnSlashWave()
    {
        if (slashWavePrefab == null || firePoint == null)
            return;

        Instantiate(slashWavePrefab, firePoint.position, Quaternion.identity);
    }

    // -----------------------------
    // Whirlwind Skill Input
    // -----------------------------
    public void OnWhirlwindSkill(InputAction.CallbackContext context)
    {
        if (!context.started)
            return;

        if (isUsingSkill)
            return;

        if (Time.time < lastWhirlwindSkillTime + whirlwindSkillCooldown)
            return;

        if (actionState != null && !actionState.TryEnterSkill())
            return;

        UseWhirlwindSkill();
    }

    private void UseWhirlwindSkill()
    {
        lastWhirlwindSkillTime = Time.time;
        isUsingSkill = true;

        if (animator != null)
        {
            animator.ResetTrigger("Attack1");
            animator.ResetTrigger("Attack2");
            animator.ResetTrigger(slashSkillTriggerName);
            animator.SetTrigger(whirlwindSkillTriggerName);
        }
        else
        {
            EndSkill();
        }
    }

    // -----------------------------
    // Dash Skill Input
    // -----------------------------
    public void OnDashSkill(InputAction.CallbackContext context)
    {
        if (!context.started)
            return;

        if (isUsingSkill)
            return;

        if (Time.time < lastDashSkillTime + DashSkillCooldown)
            return;

        if (actionState != null && !actionState.TryEnterSkill())
            return;

        UseDashSkill();
    }

    private void UseDashSkill()
    {
        lastDashSkillTime = Time.time;
        isUsingSkill = true;

        if (animator != null)
        {
            animator.ResetTrigger("Attack1");
            animator.ResetTrigger("Attack2");
            animator.ResetTrigger(slashSkillTriggerName);
            animator.SetTrigger(DashSkillTriggerName);
        }
        else
        {
            EndSkill();
        }
    }

    public void AE_EndSkill()
    {
        EndSkill();
    }

    private void EndSkill()
    {
        isUsingSkill = false;

        if (actionState != null)
            actionState.ForceIdle();
    }

    public bool IsUsingSkill => isUsingSkill;

    public float GetSlashSkillCooldown()
    {
        return slashSkillCooldown;
    }

    public float GetSlashSkillRemainingCooldown()
    {
        float remain = (lastSlashSkillTime + slashSkillCooldown) - Time.time;
        return Mathf.Max(0f, remain);
    }

    public float GetWhirlwindSkillCooldown()
    {
        return whirlwindSkillCooldown;
    }

    public float GetWhirlwindSkillRemainingCooldown()
    {
        float remain = (lastWhirlwindSkillTime + whirlwindSkillCooldown) - Time.time;
        return Mathf.Max(0f, remain);
    }

    public float GetDashSkillCooldown()
    {
        return DashSkillCooldown;
    }

    public float GetDashSkillRemainingCooldown()
    {
        float remain = (lastDashSkillTime + DashSkillCooldown) - Time.time;
        return Mathf.Max(0f, remain);
    }
}