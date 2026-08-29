using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    public enum ActionState
    {
        Idle,
        Attack,
        Skill
    }

    [SerializeField] private ActionState currentState = ActionState.Idle;

    public ActionState CurrentState => currentState;

    public bool IsIdle => currentState == ActionState.Idle;
    public bool IsAttacking => currentState == ActionState.Attack;
    public bool IsUsingSkill => currentState == ActionState.Skill;

    public bool TryEnterAttack()
    {
        if (currentState != ActionState.Idle)
            return false;

        currentState = ActionState.Attack;
        return true;
    }

    public bool TryEnterSkill()
    {
        if (currentState != ActionState.Idle)
            return false;

        currentState = ActionState.Skill;
        return true;
    }

    public void ForceIdle()
    {
        currentState = ActionState.Idle;
    }
}