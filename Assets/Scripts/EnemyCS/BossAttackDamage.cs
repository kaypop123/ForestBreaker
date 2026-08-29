using UnityEngine;

public class BossAttackDamage : MonoBehaviour
{
    [SerializeField] private int damageAmount = 2;

    private bool hasHitThisAttack = false;

    public void ResetHit()
    {
        hasHitThisAttack = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasHitThisAttack)
            return;

        PlayerState player = other.GetComponent<PlayerState>();
        if (player == null)
            return;

        player.TakeDamage(damageAmount);
        hasHitThisAttack = true;
    }
}