using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelManager : MonoBehaviour
{
    [Header("Hand and Object Data")]
    [SerializeField] private List<HandData> handDataList; // List of hand data with transforms and tubs

    public static int groundWater = 0;

    [Header("Player Settings")]
    [SerializeField] private Transform player; // Player object
    [SerializeField] private float interactionDistance = 3f; // Distance within which the player can interact

    [Header("Hand Activation Settings")]
    [SerializeField] private float handActivationInterval = 15f; // Interval between hand activations
    private int currentHandIndex = 0; // Tracks the currently activated hand
    private bool isInteracting = false; // Prevents multiple interactions simultaneously

    public float proximityRange = 5f; // Range within which the player can interact
    public string leverGameSceneName = "LeverGame"; // Name of the mini-game scene to load

    private GameObject player1;
    public bool isMiniGameActive = false; // To track if the mini-game is already active
    
    [SerializeField]private Canvas InitialSetup;
    [SerializeField]private Canvas DynamicUI;
    [SerializeField]private Canvas MiniMapUI;
    [SerializeField]private AudioSource audioSource;
    [SerializeField]private AudioClip objectiveCompleteSound;
    private static int TextStatus = 0;
    [SerializeField]private TextMeshProUGUI ObjectiveHead;
    [SerializeField]private TextMeshProUGUI ObjectiveBody;
    [SerializeField]private TextMeshProUGUI SubtitleText;

    private static bool InitialSetupDone = false;

    private void Awake2(){
        InitialSetup.enabled = true;
        DynamicUI.enabled = false;
        MiniMapUI.enabled = false;
    }
    private void Start()
    {   
        if(!InitialSetupDone){
            Awake2();
            InitialSetupDone = true;
        }
        StartCoroutine(ActivateHandsByTime());
        player1 = GameObject.FindGameObjectWithTag("Player");
        UpdateText();
    }
    private void UpdateText(){
        audioSource.PlayOneShot(objectiveCompleteSound);
        switch(TextStatus){
            case 0 : ObjectiveHead.text = "Collect Rainwater";
                     ObjectiveBody.text = "Place pits under the roof for rainwater collection(0/3)";
                     SubtitleText.text = "Let’s try to collect the rainwater and save it.";
                     break;
            case 1 : ObjectiveHead.text = "Collect Rainwater";
                     ObjectiveBody.text = "Place pits under the roof for rainwater collection(1/3)";
                     SubtitleText.text = "Rainwater is a very good source of clean drinking water";
                     break;
            case 2 : ObjectiveHead.text = "Collect Rainwater";
                     ObjectiveBody.text = "Place pits under the roof for rainwater collection(2/3)";
                     SubtitleText.text = "Rainwater can be collected and stored with ease";
                     break;
            case 3 : ObjectiveHead.text = "Prevent Soil Erosion";
                     ObjectiveBody.text = "Plant trees to stabilize soil(0/3)";
                     SubtitleText.text = "Awesome! Now the rainwater can be collected and stored for future use. Let’s check the soil.\nOh no! It’s eroding faster than expected! Quickly, plant trees to prevent soil erosion.";
                     break;
            case 4 : ObjectiveHead.text = "Prevent Soil Erosion";
                     ObjectiveBody.text = "Plant trees to stabilize soil(1/3)";
                     SubtitleText.text = "Excellent! Trees such as Coconut and Mango have deep roots that effectively anchor the soil, helping to prevent further erosion.";
                     break;
            case 5 : ObjectiveHead.text = "Prevent Soil Erosion";
                     ObjectiveBody.text = "Plant trees to stabilize soil(2/3)";
                     SubtitleText.text = "Excellent! Trees such as Coconut and Mango have deep roots that effectively anchor the soil, helping to prevent further erosion.";
                     break;
            case 6 : ObjectiveHead.text = "Build Dams";
                     ObjectiveBody.text = "Place Dams in front of lake(0/3)";
                     SubtitleText.text = "Good Job! The soil is now stabilized! Now let’s take a look at the lake.\nOh no! Our lake is overflowing. It's time to build a dam to control the overflow and prevent further flooding!";
                     break;
            case 7 : ObjectiveHead.text = "Build Dams";
                     ObjectiveBody.text = "Place Dams in front of lake(1/3)";
                     SubtitleText.text = "Dams are used to control the water in the lake";
                     break;
            case 8 : ObjectiveHead.text = "Build Dams";
                     ObjectiveBody.text = "Place Dams in front of lake(2/3)";
                     SubtitleText.text = "One dam left!";
                     break;
            case 9 : ObjectiveHead.text = "Open Dam Gates";
                     ObjectiveBody.text = "Open Dam Gates carefully(0/3)\nPress F near Dam to control opening of gates";
                     SubtitleText.text = "Now, open the dam gates. \nCareful! We need to control the outflow by releasing water periodically or it may flood the downstream region";
                     break;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            InitialSetup.enabled = false;
            DynamicUI.enabled = true;
            MiniMapUI.enabled = true;
            
        }
        CheckPlayerInteraction();
        CheckPlayerDam();
    }

    private void CheckPlayerDam()
    {
        if (Input.GetKeyDown(KeyCode.F) && !isMiniGameActive)
        {
            // Check proximity to the dam object
            if (IsPlayerNearDam())
            {
                Debug.Log("Player pressed F near Dam");
                LoadDamScene();
            }
        }
    }

    private IEnumerator ActivateHandsByTime()
    {
        while (currentHandIndex < handDataList.Count)
        {
            HandData handData = handDataList[currentHandIndex];

            if (handData.input != null && !handData.input.gameObject.activeSelf)
            {
                // Set the position and rotation explicitly before activation
                Vector3 desiredPosition = handData.input.position;

                handData.input.position = desiredPosition;

                Debug.Log($"Activating hand {currentHandIndex} at position: {desiredPosition}");

                // Activate the hand
                handData.input.gameObject.SetActive(true);
            }

            // Wait until the hand is deactivated (interaction completed)
            yield return new WaitUntil(() => !handData.input.gameObject.activeSelf);

            currentHandIndex++;

            // Wait for the specified interval before activating the next hand
            yield return new WaitForSeconds(handActivationInterval);
        }

        Debug.Log("All hands have been activated and interacted with.");
    }

    private void LoadDamScene()
    {
        isMiniGameActive = true;

        // Load the mini-game scene directly
        SceneManager.LoadScene(leverGameSceneName);
        Debug.Log("Mini-game scene loaded");
    }

    private void CheckPlayerInteraction()
    {
        if (isInteracting) return; // Prevent interaction spamming

        foreach (var handData in handDataList)
        {
            // Skip if the hand is not active in the scene
            if (handData.input == null || !handData.input.gameObject.activeSelf) continue;

            // Check if the player is within the interaction distance of the hand
            float distance = Vector3.Distance(player.position, handData.input.position);
            if (distance <= interactionDistance)
            {
                Debug.Log("Press 'E' to interact with the hand!");

                // Wait for the player to press E
                if (Input.GetKeyDown(KeyCode.E))
                {
                    StartCoroutine(HandleInteraction(handData));
                    TextStatus++;
                    UpdateText();
                }
            }
        }
    }

    private IEnumerator HandleInteraction(HandData handData)
    {
        isInteracting = true;

        groundWater += 1000;
        // Activate all Tubs linked to the hand
        foreach (Transform tub in handData.Tubs)
        {
            if (tub != null && !tub.gameObject.activeSelf)
            {
                tub.gameObject.SetActive(true); // Activate the tub GameObject at its preset position
                Debug.Log("Activated tub: " + tub.name);
            }
        }

        // After the interaction, deactivate the hand and allow the next one to be spawned
        if (handData.input != null)
        {
            handData.input.gameObject.SetActive(false); // Deactivate the hand
            Debug.Log("Deactivated hand: " + handData.input.name);
        }

        yield return new WaitForSeconds(0.5f); // Short delay to prevent interaction spamming
        isInteracting = false;
    }

    private bool IsPlayerNearDam()
    {
        // Calculate the distance between the player and the dam
        float distance = Vector3.Distance(player1.transform.position, transform.position);
        return distance <= proximityRange;
    }

    public static int GetWater()
    {
        return groundWater;
    }
}

// New HandData class to store hand and tub transforms
[System.Serializable]
public class HandData
{
    public Transform input; // The hand transform
    public List<Transform> Tubs; // List of tub transforms associated with this hand
}
