using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombatController : MonoBehaviour
{
    [Header("Anim")]
    [SerializeField] private Animator animator;
    [SerializeField] private string atk1Trig = "Attack1";
    [SerializeField] private string atk2Trig = "Attack2";

    [Header("Reference")]
    [SerializeField] private PlayerInputHandler actionState;

    private bool isAtk;
    private bool canCombo;
    private bool nextCombo;
    private int atkIdx;

    private void Awake()
    {
        if (animator == null)
            animator = GetComponent<Animator>();

        if (actionState == null)
            actionState = GetComponent<PlayerInputHandler>();
    }

    public void OnAttack(InputAction.CallbackContext ctx)
    {
        if (!ctx.started)
            return;

        if (!isAtk)
        {
            if (actionState != null && !actionState.TryEnterAttack())
                return;

            Atk1();
            return;
        }

        if (canCombo)
        {
            nextCombo = true;
        }
    }

    private void Atk1()
    {
        isAtk = true;
        canCombo = false;
        nextCombo = false;
        atkIdx = 1;

        animator.ResetTrigger(atk2Trig);
        animator.ResetTrigger("SlashSkill");
        animator.SetTrigger(atk1Trig);
    }

    private void Atk2()
    {
        isAtk = true;
        canCombo = false;
        nextCombo = false;
        atkIdx = 2;

        animator.ResetTrigger(atk1Trig);
        animator.ResetTrigger("SlashSkill");
        animator.SetTrigger(atk2Trig);
    }

    public void AE_ComboOn()
    {
        if (!isAtk) return;
        canCombo = true;
    }

    public void AE_ComboOff()
    {
        canCombo = false;
    }

    public void AE_EndAtk1()
    {
        canCombo = false;

        if (nextCombo)
            Atk2();
        else
            EndAtk();
    }

    public void AE_EndAtk2()
    {
        EndAtk();
    }

    private void EndAtk()
    {
        isAtk = false;
        canCombo = false;
        nextCombo = false;
        atkIdx = 0;

        if (actionState != null)
            actionState.ForceIdle();
    }

    public void CancelAttackByHit()
    {
        isAtk = false;
        canCombo = false;
        nextCombo = false;
        atkIdx = 0;

        animator.ResetTrigger(atk1Trig);
        animator.ResetTrigger(atk2Trig);

        if (actionState != null)
            actionState.ForceIdle();
    }

    public bool IsAttacking => isAtk;
}