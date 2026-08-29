using UnityEngine;

public class EnemyBossSfx : MonoBehaviour
{

    [Header("Combat")]
    public AudioClip fireSlashSfx;
    public AudioClip fireBallSfx;

   
    public void FireSlashSkill()
    {
        SoundManager.Instance.PlaySFX(fireSlashSfx);
    }
    public void FireBallSkill()
    {
        SoundManager.Instance.PlaySFX(fireBallSfx);
    }
}
