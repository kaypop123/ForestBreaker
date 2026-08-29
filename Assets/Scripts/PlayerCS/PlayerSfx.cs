using UnityEngine;

public class PlayerSfx : MonoBehaviour
{
    [Header("Movement")]
    public AudioClip walkSfx;
    public AudioClip dashSfx;

    [Header("Combat")]
    public AudioClip hitSfx;
    public AudioClip attack1Sfx;
    public AudioClip blockSfx;

    [Header("Skill")]
    public AudioClip dashSkillSfx;
    public AudioClip slashSkillSfx;

    public void PlayWalk()
    {
        SoundManager.Instance.PlaySFX(walkSfx);
    }
    public void PlayDash()
    {
        SoundManager.Instance.PlaySFX(dashSfx);
    }

    public void PlayHit()
    {
        SoundManager.Instance.PlaySFX(hitSfx);
    }
    public void PlayBlockSfx()
    {
        SoundManager.Instance.PlaySFX(blockSfx);
    }

    public void PlayAttack1()
    {
        SoundManager.Instance.PlaySFX(attack1Sfx);
    }


    public void PlayDashSkill()
    {
        SoundManager.Instance.PlaySFX(dashSkillSfx);
    }

    public void PlaySlashSkill()
    {
        SoundManager.Instance.PlaySFX(slashSkillSfx);
    }
}