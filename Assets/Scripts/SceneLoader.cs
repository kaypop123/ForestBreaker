using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [Header("Scene")]
    [SerializeField] private string gameSceneName = "GameScene";

    public void OnClickStartButton()
    {
        SceneManager.LoadScene(gameSceneName);
    }
}