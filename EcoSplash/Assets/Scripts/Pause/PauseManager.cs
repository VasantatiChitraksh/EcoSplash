using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private Canvas controlsCanvas;
    private bool isPaused = false;

    void Start()
    {   
        controlsCanvas.enabled = false;
    }

    void Update()
    {   
        if(Input.GetKeyDown(KeyCode.L)){
            SceneManager.LoadScene("HomeMenu");
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void PauseGame()
    {
        controlsCanvas.enabled = true;
        isPaused = true;
    }

    public void ResumeGame()
    {
        controlsCanvas.enabled = false;
        isPaused = false;
    }
}