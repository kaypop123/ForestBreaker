using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerBlockingController : MonoBehaviour
{
    [Header("설정")]
    [SerializeField] private string enemyTag = "Enemy";
    [SerializeField] private string blockTriggerName = "Block";
    [SerializeField] private float knockbackForce = 7f;
    [SerializeField] private float knockbackDuration = 0.2f;

    [Header("Player넉백")]
    [SerializeField] private float playerPushbackForce = 4f;
    [SerializeField] private float playerPushbackDuration = 0.12f;

    [Header("막기 대기시간")]
    [SerializeField] private float blockCooldown = 1.0f;

    [Header("애니메이터")]
    [SerializeField] private Animator animator;

    private Rigidbody2D rb;

    private bool isTouchingEnemy;
    private bool canBlock = true;
    private EnemyLeftMovement currentEnemy;

    private float lastBlockUseTime = -999f;
    private Coroutine playerPushbackCoroutine;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void OnBlockCheck(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        if (!canBlock) return;

        if (isTouchingEnemy)
        {
            PerformBlock();
        }
    }

    private void PerformBlock()
    {
        lastBlockUseTime = Time.time;

        if (animator != null)
        {
            animator.SetTrigger(blockTriggerName);
        }

        StartCoroutine(BlockCooldownRoutine());
    }

    // 애니메이션 이벤트에서 호출
    public void AE_Knockback()
    {
        if (isTouchingEnemy && currentEnemy != null)
        {
            Debug.Log($"{currentEnemy.name}에게 애니메이션 타이밍 맞춰 넉백 시전!");
            currentEnemy.GetKnockback(knockbackForce, knockbackDuration);

            // 플레이어도 뒤로 밀리기
            ApplyPlayerPushback();
        }
    }

    private void ApplyPlayerPushback()
    {
        if (rb == null) return;

        float dir = transform.localScale.x > 0 ? -1f : 1f;
        if (playerPushbackCoroutine != null)
        {
            StopCoroutine(playerPushbackCoroutine);
        }

        playerPushbackCoroutine = StartCoroutine(PlayerPushbackRoutine(dir));
    }

    private IEnumerator PlayerPushbackRoutine(float dir)
    {
        // 기존 속도 초기화
        rb.linearVelocity = Vector2.zero;

        float timer = 0f;

        while (timer < playerPushbackDuration)
        {
            rb.linearVelocity = new Vector2(dir * playerPushbackForce, rb.linearVelocity.y);
            timer += Time.deltaTime;
            yield return null;
        }

        // 밀림 종료 후 x축 속도만 정리
        rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
    }

    public float GetBlockCooldown()
    {
        return blockCooldown;
    }

    public float GetRemainingCooldown()
    {
        float remain = (lastBlockUseTime + blockCooldown) - Time.time;
        return Mathf.Max(0f, remain);
    }

    private IEnumerator BlockCooldownRoutine()
    {
        canBlock = false;
        yield return new WaitForSeconds(blockCooldown);
        canBlock = true;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(enemyTag))
        {
            isTouchingEnemy = true;

            if (currentEnemy == null)
            {
                currentEnemy = collision.collider.GetComponentInParent<EnemyLeftMovement>();
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(enemyTag))
        {
            isTouchingEnemy = false;
            currentEnemy = null;
        }
    }
}