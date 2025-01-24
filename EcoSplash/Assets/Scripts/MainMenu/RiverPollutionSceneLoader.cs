using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelMenuScript : MonoBehaviour
{
    public string LoadingScene;
    public Button IndustrialSceneLoad;
    public Button FloodSceneLoad;
    public Button DroughtSceneLoad;

    void Start()
    {
        // Add click listeners to each button
        if (IndustrialSceneLoad != null)
            IndustrialSceneLoad.onClick.AddListener(() => LoadScene("IndustrialLevelStart"));

        if (FloodSceneLoad != null)
            FloodSceneLoad.onClick.AddListener(() => LoadScene("FloodScene"));

        if (DroughtSceneLoad != null)
            DroughtSceneLoad.onClick.AddListener(() => LoadScene("DroughtScene"));
    }

    public void LoadScene(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
    }
}