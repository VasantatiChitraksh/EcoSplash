using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader
{
    public enum Scene
    {
        LeverGame,
        LoadingScene,
        FloodScene,
    }

    private static Scene targetScene;

    public static void Load(Scene targetScene)
    {
        Loader.targetScene = targetScene;

        // Load the LoadingScene while preparing the target scene
        SceneManager.LoadScene(Scene.LoadingScene.ToString());
    }

    // This function will be called from the LoadingScene to load the actual target scene
    public static void LoaderCallBack()
    {
        // Load the target scene based on the value of targetScene
        SceneManager.LoadScene(targetScene.ToString());
    }
}
