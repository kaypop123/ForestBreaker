using UnityEngine;

public class EnemyAnim : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D parentRb;

    [SerializeField] private string speedParamName = "Walk";
    [SerializeField] private float threshold = 0.1f;
    [SerializeField] private string dieTriggerName = "Die";
    [SerializeField] private string hitTriggerName = "Hit";

    private bool isDead = false;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        parentRb = GetComponentInParent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        isDead = false;

        if (anim != null)
        {
            anim.ResetTrigger(dieTriggerName);
            anim.ResetTrigger(hitTriggerName);
            anim.SetFloat(speedParamName, 0f);

            anim.Play("Idle", 0, 0f);
        }
    }

    private void Update()
    {
        if (parentRb == null || anim == null) return;

        OnAnimatorMove();
    }

    private void OnAnimatorMove()
    {
        float currentSpeed = Mathf.Abs(parentRb.linearVelocity.x);
        anim.SetFloat(speedParamName, currentSpeed);
    }

    public void PlayHit()
    {
        if (isDead) return;

        if (anim != null)
        {
            anim.SetTrigger(hitTriggerName);
        }
    }

    public void PlayDie()
    {
        if (isDead) return;

        isDead = true;

        if (anim != null)
        {
            anim.SetTrigger(dieTriggerName);
        }
    }
}