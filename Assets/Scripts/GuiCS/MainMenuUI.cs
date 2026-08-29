using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject equipPanel;

    private void Start()
    {
        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(true);

        if (equipPanel != null)
            equipPanel.SetActive(false);
    }

    public void OnClickEquipButton()
    {
        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(false);

        if (equipPanel != null)
            equipPanel.SetActive(true);
    }

    public void OnClickEquipCloseButton()
    {
        if (equipPanel != null)
            equipPanel.SetActive(false);

        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(true);
    }

    public void OnClickExitButton()
    {

        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}