using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenuUI;
    [SerializeField] private Canvas controlsCanvas;
    [SerializeField] private Button controlsButton;
    [SerializeField] private Button mainMenuButton;
    private bool isPaused = false;

    void Start()
    {   
        controlsCanvas.enabled = false;
        pauseMenuUI.SetActive(false);
        controlsButton.onClick.AddListener(HandleControls);
        mainMenuButton.onClick.AddListener(QuitToMainMenu);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        controlsCanvas.enabled = false;
        Time.timeScale = 1f;
        isPaused = false;
    }

    private void HandleControls()
    {
        controlsCanvas.enabled = true;
    }

    private void QuitToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("HomeMenu");
    }
}