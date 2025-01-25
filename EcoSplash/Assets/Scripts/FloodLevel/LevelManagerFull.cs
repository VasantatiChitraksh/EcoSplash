using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

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
    [SerializeField]private Canvas DynamicUI;
    [SerializeField]private Canvas MiniMapUI;
    [SerializeField]private AudioSource audioSource;
    [SerializeField]private AudioClip objectiveCompleteSound;
    private static int TextStatus = 0;
    [SerializeField]private TextMeshProUGUI ObjectiveHead;
    [SerializeField]private TextMeshProUGUI ObjectiveBody;
    [SerializeField]private TextMeshProUGUI SubtitleText;

    private static bool InitialSetupDone = false;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep the object across scenes
        }
    }
    
    private void UpdateText(){
        audioSource.PlayOneShot(objectiveCompleteSound);
        switch(TextStatus){
            case 0 : ObjectiveHead.text = "Open Dam Gates";
                     ObjectiveBody.text = "Open Dam Gates carefully(1/3)\nPress F near Dam to control opening of gates";
                     SubtitleText.text = "Now, open the dam gates. \nCareful! We need to control the outflow by releasing water periodically or it may flood the downstream region";
                     break;
            case 1 : ObjectiveHead.text = "Open Dam Gates";
                     ObjectiveBody.text = "Open Dam Gates carefully(2/3)\nPress F near Dam to control opening of gates";
                     SubtitleText.text = "Now, open the dam gates. \nCareful! We need to control the outflow by releasing water periodically or it may flood the downstream region";
                     break;
        }
    }


    private void Start()
    {
        player1 = GameObject.FindGameObjectWithTag("Player");
        OnMiniGameCompleted += UpdateMiniGameCounter;
        UpdateText();
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
                TextStatus++;
                UpdateText();
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
