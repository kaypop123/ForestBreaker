using UnityEngine;
using UnityEngine.UI;

public class PlayerHPUI : MonoBehaviour
{
    [SerializeField] private PlayerState playerState;
    [SerializeField] private Image hpFillImage;

    private void Start()
    {
        UpdateHPUI();
    }

    private void Update()
    {
        UpdateHPUI();
    }

    private void UpdateHPUI()
    {
        if (playerState == null || hpFillImage == null)
            return;

        float ratio = (float)playerState.CurrentHealth / playerState.MaxHealth;
        hpFillImage.fillAmount = ratio;
    }
}