using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class PlayerDashController : MonoBehaviour
{
    [Header("대쉬")]
    [SerializeField] private float dashSpeed = 14f;
    [SerializeField] private float dashDuration = 0.12f;
    [SerializeField] private float dashCooldown = 0.35f;
    [SerializeField] private string enemyTag = "Enemy";

    [Header("애니메이션")]
    [SerializeField] private Animator animator;
    [SerializeField] private string dashTriggerName = "Dash";

    [Header("애니메이션컨")]
    [SerializeField] private PlayerController playerController;

    private Rigidbody2D rb;
    private Collider2D playerCol;

    private bool isDashing;
    private bool canDash = true;
    private Coroutine dashCoroutine;
    private float originalGravityScale;

    private float lastDashUseTime = -999f;

    public bool IsDashing => isDashing;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCol = GetComponent<Collider2D>();

        if (animator == null)
            animator = GetComponent<Animator>();

        if (playerController == null)
            playerController = GetComponent<PlayerController>();

        originalGravityScale = rb.gravityScale;
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        if (!canDash || isDashing) return;

        if (playerController != null && playerController.IsMoving())
        {
            Debug.Log("강제 이동 중이라 대시 불가");
            return;
        }

        if (IsTouchingEnemy())
        {
            Debug.Log("적과 접촉 중이라 대시 불가");
            return;
        }

        dashCoroutine = StartCoroutine(DashRightCoroutine());
    }

    public float GetDashCooldown()
    {
        return dashCooldown;
    }

    public float GetRemainingCooldown()
    {
        float remain = (lastDashUseTime + dashCooldown) - Time.time;
        return Mathf.Max(0f, remain);
    }

    private bool IsTouchingEnemy()
    {
        Collider2D[] hits = Physics2D.OverlapBoxAll(
            playerCol.bounds.center,
            playerCol.bounds.size,
            0f
        );

        foreach (Collider2D hit in hits)
        {
            if (hit == playerCol) continue;

            if (hit.CompareTag(enemyTag))
            {
                return true;
            }
        }

        return false;
    }

    private IEnumerator DashRightCoroutine()
    {
        isDashing = true;
        canDash = false;
        lastDashUseTime = Time.time;

        if (animator != null)
            animator.SetTrigger(dashTriggerName);

        rb.gravityScale = 0f;

        float elapsed = 0f;
        while (elapsed < dashDuration)
        {
            rb.linearVelocity = new Vector2(dashSpeed, 0f);
            elapsed += Time.deltaTime;
            yield return null;
        }

        EndDash();

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    private void EndDash()
    {
        rb.gravityScale = originalGravityScale;
        rb.linearVelocity = Vector2.zero;
        isDashing = false;
        dashCoroutine = null;
    }

    private void StopDashByEnemy()
    {
        if (!isDashing) return;

        if (dashCoroutine != null)
        {
            StopCoroutine(dashCoroutine);
            dashCoroutine = null;
        }

        EndDash();
        StartCoroutine(DashCooldownRoutine());
    }

    private IEnumerator DashCooldownRoutine()
    {
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isDashing) return;

        if (collision.collider.CompareTag(enemyTag))
        {
            StopDashByEnemy();
        }
    }
}