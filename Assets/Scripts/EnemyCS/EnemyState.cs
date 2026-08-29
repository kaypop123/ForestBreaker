using UnityEngine;
using System.Collections;

public class EnemyState : MonoBehaviour
{
    [Header("상태")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private float deactivateDelay = 1.5f;

    [Header("Hit Effect")]
    [SerializeField] private GameObject hitEffectPrefab;
    [SerializeField] private Transform hitEffectPoint;
    [SerializeField] private float hitEffectLifeTime = 1f;

    [Header("아이템 드롭")]
    [SerializeField] private GameObject dropItemPrefab;
    [SerializeField] private Transform dropPoint;
    [SerializeField] private bool dropOnDeath = true;

    private int currentHealth;
    private bool isDead = false;

    public bool IsDead => isDead;

    private EnemyAnim[] anims;
    private Collider2D[] colliders;
    private EnemySpawner spawner;

    private void Awake()
    {
        anims = GetComponentsInChildren<EnemyAnim>(true);
        colliders = GetComponentsInChildren<Collider2D>(true);
        spawner = Object.FindFirstObjectByType<EnemySpawner>();
    }

    private void OnEnable()
    {
        ResetEnemy();
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        Debug.Log($"적이 {damage}의 데미지를 입음 남은 체력: {currentHealth}");

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            isDead = true;
            StartCoroutine(DieRoutine());
            return;
        }

        PlayHitEffect();

        foreach (EnemyAnim anim in anims)
        {
            anim.PlayHit();
        }
    }

    private void PlayHitEffect()
    {
        if (hitEffectPrefab == null) return;

        Vector3 spawnPosition = transform.position;

        if (hitEffectPoint != null)
            spawnPosition = hitEffectPoint.position;

        GameObject effect = Instantiate(hitEffectPrefab, spawnPosition, Quaternion.identity);
        Destroy(effect, hitEffectLifeTime);
    }

    private IEnumerator DieRoutine()
    {

        foreach (EnemyAnim anim in anims)
        {
            anim.PlayDie();
        }

        foreach (Collider2D col in colliders)
        {
            col.enabled = false;
        }

        DropItem();

        if (spawner != null)
        {
            spawner.NotifyEnemyDead();
        }

        yield return new WaitForSeconds(deactivateDelay);

        gameObject.SetActive(false);
    }

    private void DropItem()
    {
        if (!dropOnDeath)
            return;

        if (dropItemPrefab == null)
            return;

        Vector3 spawnPosition = transform.position;

        if (dropPoint != null)
            spawnPosition = dropPoint.position;

        Instantiate(dropItemPrefab, spawnPosition, Quaternion.identity);
    }

    private void ResetEnemy()
    {
        isDead = false;
        currentHealth = maxHealth;

        foreach (Collider2D col in colliders)
        {
            col.enabled = true;
        }
    }
}