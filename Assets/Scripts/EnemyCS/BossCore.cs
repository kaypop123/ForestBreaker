using UnityEngine;

public class BossCore : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target;

    [Header("Range")]
    [SerializeField] private float detectRange = 8f;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float skillRange = 7f;

    [Header("References")]
    [SerializeField] private BossAttack attackController;
    [SerializeField] private BossSkill skillController;
    [SerializeField] private EnemyState bossHealth;

    private void Awake()
    {
        if (attackController == null)
            attackController = GetComponent<BossAttack>();

        if (skillController == null)
            skillController = GetComponent<BossSkill>();

        if (bossHealth == null)
            bossHealth = GetComponent<EnemyState>();
    }
    private void Start()
    {
        FindTarget();
    }
    private void Update()
    {
        if (bossHealth != null && bossHealth.IsDead) return;

        // 타겟이 없으면 찾기 시도
        if (target == null)
        {
            FindTarget();
            if (target == null) return; // 여전히 없으면 리턴
        }

        if (IsBusy()) return;

        if (bossHealth != null && bossHealth.IsDead)
            return;

        if (target == null)
            return;

        if (IsBusy())
            return;

        float distance = Vector2.Distance(transform.position, target.position);

        if (distance > detectRange)
            return;

        if (distance <= skillRange && skillController != null && skillController.CanUseSkill())
        {
            skillController.TryUseSkill();
            return;
        }

        if (distance <= attackRange && attackController != null && attackController.CanAttack())
        {
            attackController.TryAttack();
            return;
        }
    }
    private void FindTarget()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            target = player.transform;
        }
    }
    private bool IsBusy()
    {
        if (attackController != null && attackController.IsAttacking)
            return true;

        if (skillController != null && skillController.IsUsingSkill)
            return true;

        return false;
    }
}