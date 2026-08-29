using UnityEngine;

public class EquipmentDataManager : MonoBehaviour
{
    public static EquipmentDataManager Instance;

    private const string ArmorOwnedKey = "ArmorOwned";
    private const string ArmorEquippedKey = "ArmorEquipped";

    public bool IsArmorOwned => PlayerPrefs.GetInt(ArmorOwnedKey, 0) == 1;
    public bool IsArmorEquipped => PlayerPrefs.GetInt(ArmorEquippedKey, 0) == 1;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UnlockArmor()
    {
        PlayerPrefs.SetInt(ArmorOwnedKey, 1);
        PlayerPrefs.Save();

        Debug.Log("∞©ø  »πµÊ øœ∑·");
        RefreshAllArmorSlots();
    }

    public void EquipArmor()
    {
        if (!IsArmorOwned)
        {
            Debug.Log("∞©ø ¿ª æ∆¡˜ »πµÊ«œ¡ˆ æ ¿Ω");
            return;
        }

        PlayerPrefs.SetInt(ArmorEquippedKey, 1);
        PlayerPrefs.Save();

        Debug.Log("∞©ø  ¿Â¬¯ øœ∑·");
        RefreshAllArmorSlots();
    }

    public void UnequipArmor()
    {
        PlayerPrefs.SetInt(ArmorEquippedKey, 0);
        PlayerPrefs.Save();

        Debug.Log("∞©ø  ¿Â¬¯ «ÿ¡¶");
        RefreshAllArmorSlots();
    }

    public void ResetArmorData()
    {
        PlayerPrefs.DeleteKey(ArmorOwnedKey);
        PlayerPrefs.DeleteKey(ArmorEquippedKey);
        PlayerPrefs.Save();

        Debug.Log("∞©ø  µ•¿Ã≈Õ √ ±‚»≠ øœ∑·");
        RefreshAllArmorSlots();
    }

    private void RefreshAllArmorSlots()
    {
        ArmorSlotUI[] slotUIs = FindObjectsByType<ArmorSlotUI>(FindObjectsSortMode.None);

        foreach (ArmorSlotUI slotUI in slotUIs)
        {
            if (slotUI != null)
                slotUI.RefreshUI();
        }
    }
}