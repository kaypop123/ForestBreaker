using UnityEngine;
using System.Collections;

public class PlayerState : MonoBehaviour
{
    [Header("체력")]
    [SerializeField] private int maxHealth = 10;
    [SerializeField] private Animator animator;
    [SerializeField] private string hitTriggerName = "Hit";

    [Header("피격 FX")]
    [SerializeField] private GameObject hitFxPrefab;
    [SerializeField] private Transform hitFxPoint;
    [SerializeField] private float hitFxDestroyTime = 1f;

    [Header("무적 시간")]
    [SerializeField] private float invincibleDuration = 1.0f;
    [SerializeField] private float blinkInterval = 0.1f;

    [Header("깜빡임 플레이어")]
    [SerializeField] private SpriteRenderer[] blinkRenderers;

    [Header("카메라 쉐이크")]
    [SerializeField] private CameraShakeSystem cameraShakeSystem;

    [Header("사망UI")]
    [SerializeField] private GameOverUI gameOverUI;

    private int currentHealth;
    private PlayerCombatController combat;

    private bool isInvincible;
    private Coroutine invincibleCoroutine;

    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;
    public bool IsInvincible => isInvincible;

    private void Awake()
    {
        currentHealth = maxHealth;

        if (animator == null)
            animator = GetComponent<Animator>();

        combat = GetComponent<PlayerCombatController>();

        if (blinkRenderers == null || blinkRenderers.Length == 0)
        {
            blinkRenderers = GetComponentsInChildren<SpriteRenderer>();
        }

        if (cameraShakeSystem == null)
            cameraShakeSystem = FindFirstObjectByType<CameraShakeSystem>();
        if (gameOverUI == null)
            gameOverUI = Object.FindFirstObjectByType<GameOverUI>();
    }

    public void TakeDamage(int damage)
    {
        if (currentHealth <= 0)
            return;

        if (isInvincible)
            return;

        currentHealth -= damage;

        if (currentHealth < 0)
            currentHealth = 0;

        if (combat != null && combat.IsAttacking)
            combat.CancelAttackByHit();

        if (animator != null)
            animator.SetTrigger(hitTriggerName);

        SpawnHitFx();

        if (cameraShakeSystem != null)
            cameraShakeSystem.PlayShake();

        Debug.Log($"플레이어 체력: {currentHealth}/{maxHealth}");

        if (currentHealth == 0)
        {
            Die();
            return;
        }

        if (invincibleCoroutine != null)
        {
            StopCoroutine(invincibleCoroutine);
        }

        invincibleCoroutine = StartCoroutine(InvincibleRoutine());
    }

    private void SpawnHitFx()
    {
        if (hitFxPrefab == null)
            return;

        Vector3 spawnPosition = hitFxPoint != null ? hitFxPoint.position : transform.position;

        GameObject fx = Instantiate(hitFxPrefab, spawnPosition, Quaternion.identity);
        Destroy(fx, hitFxDestroyTime);
    }

    private IEnumerator InvincibleRoutine()
    {
        isInvincible = true;

        float elapsed = 0f;
        bool visible = true;

        while (elapsed < invincibleDuration)
        {
            visible = !visible;
            SetRenderersVisible(visible);

            yield return new WaitForSeconds(blinkInterval);
            elapsed += blinkInterval;
        }

        SetRenderersVisible(true);

        isInvincible = false;
        invincibleCoroutine = null;
    }

    private void SetRenderersVisible(bool visible)
    {
        if (blinkRenderers == null || blinkRenderers.Length == 0)
            return;

        for (int i = 0; i < blinkRenderers.Length; i++)
        {
            if (blinkRenderers[i] != null)
            {
                blinkRenderers[i].enabled = visible;
            }
        }
    }

    private void Die()
    {
        Debug.Log("플레이어 사망");
        if (gameOverUI != null)
            gameOverUI.ShowGameOver();
        SetRenderersVisible(true);

        gameObject.SetActive(false);

    }
}