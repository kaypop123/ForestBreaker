using UnityEngine;
using UnityEngine.UI;

public class ArmorSlotUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Button armorButton;
    [SerializeField] private Image iconImage;
    [SerializeField] private GameObject lockImage;
    [SerializeField] private GameObject equippedText;

    [Header("Icon")]
    [SerializeField] private Sprite armorIcon;

    [Header("Color")]
    [SerializeField] private Color ownedColor = Color.white;
    [SerializeField] private Color lockedColor = new Color(0.35f, 0.35f, 0.35f, 1f);

    private void Start()
    {
        if (armorButton != null)
        {
            armorButton.onClick.AddListener(OnClickArmorSlot);
        }

        RefreshUI();
    }

    private void OnEnable()
    {
        RefreshUI();
    }

    public void OnClickArmorSlot()
    {
        if (EquipmentDataManager.Instance == null)
            return;

        if (!EquipmentDataManager.Instance.IsArmorOwned)
        {
            Debug.Log("°©¿ÊÀ» ¾ÆÁ÷ È¹µæÇÏÁö ¾Ê¾Ò½À´Ï´Ù.");
            return;
        }

        if (EquipmentDataManager.Instance.IsArmorEquipped)
        {
            EquipmentDataManager.Instance.UnequipArmor();
        }
        else
        {
            EquipmentDataManager.Instance.EquipArmor();
        }

        RefreshUI();
    }

    public void RefreshUI()
    {
        if (EquipmentDataManager.Instance == null)
        {
            Debug.Log("EquipmentDataManager.Instance ¾øÀ½");
            return;
        }

        bool isOwned = EquipmentDataManager.Instance.IsArmorOwned;
        bool isEquipped = EquipmentDataManager.Instance.IsArmorEquipped;

        Debug.Log($"RefreshUI È£ÃâµÊ / isOwned: {isOwned}, isEquipped: {isEquipped}");

        if (iconImage != null)
        {
            if (armorIcon != null)
                iconImage.sprite = armorIcon;

            iconImage.enabled = isOwned;
        }

        if (lockImage != null)
            lockImage.SetActive(!isOwned);

        if (equippedText != null)
            equippedText.SetActive(isOwned && isEquipped);

        if (armorButton != null)
            armorButton.interactable = true;
    }
}