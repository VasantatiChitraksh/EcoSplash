using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManagerFull : MonoBehaviour
{
    public static LevelManagerFull Instance; // Singleton instance for easy access

    public event System.Action<int> OnMiniGameCompleted;

    [Header("Player Settings")]
    public float proximityRange = 5f;
    public string leverGameSceneName = "LeverGame";

    private string end = "FloodSceneFinal";
    private GameObject player1;
    public bool isMiniGameActive = false;
    private int miniGameCounter = 1;
    public int miniGameCompletionThreshold = 3;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep the object across scenes
        }
        else
        {
            Destroy(gameObject); // Ensure only one instance exists
        }
    }

    private void Start()
    {
        player1 = GameObject.FindGameObjectWithTag("Player");
        OnMiniGameCompleted += UpdateMiniGameCounter;
    }

    private void Update()
    {
        CheckPlayerDam();
    }

    private void CheckPlayerDam()
    {
        if (Input.GetKeyDown(KeyCode.F) && !isMiniGameActive)
        {
            if (IsPlayerNearDam())
            {
                Debug.Log("Player pressed F near Dam");
                LoadDamScene();
            }
        }
    }

    private void LoadDamScene()
    {
        isMiniGameActive = true;
        SceneManager.LoadScene(leverGameSceneName);
        Debug.Log("Mini-game scene loaded");
    }

    private bool IsPlayerNearDam()
    {
        float distance = Vector3.Distance(player1.transform.position, transform.position);
        return distance <= proximityRange;
    }

    private void UpdateMiniGameCounter(int completedGames)
    {
        miniGameCounter = completedGames;
        Debug.Log($"Mini-games completed: {miniGameCounter}");

        if (miniGameCounter >= miniGameCompletionThreshold)
        {
            GameCompleted();
        }
    }

    public void NotifyMiniGameCompleted()
    {
        // Trigger the event to update the mini-game counter
        OnMiniGameCompleted?.Invoke(miniGameCounter + 1);
    }

    private void GameCompleted()
    {
        SceneManager.LoadScene(end);
        Debug.Log("Game completed! All mini-games finished.");
    }
}
