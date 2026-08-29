using UnityEngine;
using UnityEngine.SceneManagement;

public class TimeLineSceneManager : MonoBehaviour
{
    [Header("Scene")]
    [SerializeField] private string gameSceneName = "BossScene";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SceneManager.LoadScene(gameSceneName);
    }
}
