using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CooldownUI : MonoBehaviour
{
    public enum CooldownType
    {
        Dash,
        Block,
        SlashSkill,
        WhirlwindSkill,
        DashSkill
    }

    [Header("UI")]
    [SerializeField] private Image cooldownOverlay;
    [SerializeField] private TextMeshProUGUI cooldownText;

    [Header("타입")]
    [SerializeField] private CooldownType cooldownType;

    [Header("연결")]
    [SerializeField] private PlayerDashController dashController;
    [SerializeField] private PlayerBlockingController blockController;
    [SerializeField] private PlayerSkillController skillController;

    private void Update()
    {
        float remain = 0f;
        float total = 0f;

        switch (cooldownType)
        {
            case CooldownType.Dash:
                if (dashController == null) return;
                remain = dashController.GetRemainingCooldown();
                total = dashController.GetDashCooldown();
                break;

            case CooldownType.Block:
                if (blockController == null) return;
                remain = blockController.GetRemainingCooldown();
                total = blockController.GetBlockCooldown();
                break;

            case CooldownType.SlashSkill:
                if (skillController == null) return;
                remain = skillController.GetSlashSkillRemainingCooldown();
                total = skillController.GetSlashSkillCooldown();
                break;

            case CooldownType.WhirlwindSkill:
                if (skillController == null) return;
                remain = skillController.GetWhirlwindSkillRemainingCooldown();
                total = skillController.GetWhirlwindSkillCooldown();
                break;

            case CooldownType.DashSkill:
                if (skillController == null) return;
                remain = skillController.GetDashSkillRemainingCooldown();
                total = skillController.GetDashSkillCooldown();
                break;
        }

        if (cooldownOverlay == null)
            return;

        if (remain > 0f && total > 0f)
        {
            float ratio = remain / total;
            cooldownOverlay.fillAmount = ratio;

            if (cooldownText != null)
            {
                cooldownText.gameObject.SetActive(true);

                if (total < 1f)
                    cooldownText.text = remain.ToString("F1");
                else
                    cooldownText.text = Mathf.Ceil(remain).ToString();
            }
        }
        else
        {
            cooldownOverlay.fillAmount = 0f;

            if (cooldownText != null)
            {
                cooldownText.text = "";
                cooldownText.gameObject.SetActive(false);
            }
        }
    }
}