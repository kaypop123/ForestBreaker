using UnityEngine;

public class PlayerContactDamage : MonoBehaviour
{
    [SerializeField] private int damageAmount = 1;
    [SerializeField] private float damageInterval = 0.5f;
    [SerializeField] private string wallTag = "Wall";
    [SerializeField] private string enemyDamageTag = "EnemyDamage";
    [SerializeField] private PlayerState playerHealth;

    private int wallContactCount;
    private int enemyDamageContactCount;

    private float lastDamageTime;

    private void Awake()
    {
        if (playerHealth == null)
            playerHealth = GetComponent<PlayerState>();
    }

    private void Update()
    {
        if (wallContactCount <= 0 || enemyDamageContactCount <= 0)
            return;

        if (Time.time < lastDamageTime + damageInterval)
            return;

        if (playerHealth == null)
            return;

        playerHealth.TakeDamage(damageAmount);
        lastDamageTime = Time.time;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(wallTag))
            wallContactCount++;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(wallTag))
            wallContactCount--;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(enemyDamageTag))
            enemyDamageContactCount++;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(enemyDamageTag))
            enemyDamageContactCount--;
    }
}