using UnityEngine;

public class PlayerAttackTrigger : MonoBehaviour
{
    public int damageAmount = 20;
    [Header("Camera Shake")]
    [SerializeField] private CameraShakeSystem cameraShakeSystem;

    private bool hasShakenThisAttack = false;

    private void Awake()
    {
        if (cameraShakeSystem == null && Camera.main != null)
        {
            cameraShakeSystem = Camera.main.GetComponent<CameraShakeSystem>();
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"{other.name} Ãæµ¹");
        EnemyState enemy = other.GetComponent<EnemyState>();

        if (enemy != null)
        {
            enemy.TakeDamage(damageAmount);
            cameraShakeSystem.PlayShake();
        }
    }
}
