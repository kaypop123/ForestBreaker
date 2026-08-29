using UnityEngine;
using System.Collections;
public class BossSkill : MonoBehaviour
{
    [Header("Skill")]
    [SerializeField] private float skillCooldown = 5f;
    [SerializeField] private float skillDuration = 1.2f;

    [Header("Animation")]
    [SerializeField] private Animator animator;
    [SerializeField] private string skillTriggerName = "Skill";

    [Header("Spawn")]
    [SerializeField] private GameObject skillPrefab;
    [SerializeField] private Transform skillSpawnPoint;

    private float lastSkillTime;
    private bool isUsingSkill;

    public bool IsUsingSkill => isUsingSkill;

    private void Awake()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
    }
    private void Start()
    {

        lastSkillTime = Time.time - skillCooldown + 2f;
    }
    public bool CanUseSkill()
    {
        return !isUsingSkill && Time.time >= lastSkillTime + skillCooldown;
    }

    public void TryUseSkill()
    {
        if (!CanUseSkill())
            return;

        StartCoroutine(SkillRoutine());
    }

    private IEnumerator SkillRoutine()
    {
        isUsingSkill = true;
        lastSkillTime = Time.time;

        if (animator != null)
            animator.SetTrigger(skillTriggerName);

        yield return new WaitForSeconds(skillDuration);

        isUsingSkill = false;
    }

    // 애니메이션 이벤트에서 호출
    public void CastSkill()
    {
        if (skillPrefab == null || skillSpawnPoint == null)
            return;

        Instantiate(skillPrefab, skillSpawnPoint.position, skillSpawnPoint.rotation);
        GameObject fireball = Instantiate(skillPrefab, skillSpawnPoint.position, skillSpawnPoint.rotation);

        Destroy(fireball, 3f); 
    }
}