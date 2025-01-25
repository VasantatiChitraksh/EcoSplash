using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelManagerI : MonoBehaviour
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
    public string leverGameSceneName = "FishingMiniGame"; // Name of the mini-game scene to load

    private GameObject player1;
    public bool isMiniGameActive = false; // To track if the mini-game is already active

    
    private static int TextStatus = 0;
    [SerializeField]private TextMeshProUGUI ObjectiveHead;
    [SerializeField]private TextMeshProUGUI ObjectiveBody;
    [SerializeField]private TextMeshProUGUI SubtitleText;

    [SerializeField]private AudioSource audioSource;
    [SerializeField]private AudioClip objectiveCompleteSound;
    [SerializeField]private Canvas InitialSetup;
    [SerializeField]private Canvas DynamicUI;
    [SerializeField]private Canvas MiniMapUI;

    private static bool isInitialDone = false;
    
    private void UpdateText(){
        audioSource.PlayOneShot(objectiveCompleteSound);
        switch(TextStatus){
            case 0 : ObjectiveHead.text = "Make Factory Eco-Friendly";
                     ObjectiveBody.text = "Build a chimney\nBuild a waste recycler";
                     SubtitleText.text = "Oh no! The river water is contaminated!\nThe factory is dumping its waste into the river. Let's work on making the factory eco-friendly!";
                     break;
            case 1 : ObjectiveHead.text = "Make Factory Eco-Friendly";
                     ObjectiveBody.text = "Build a waste recycler\nBuild a waste pit";
                     SubtitleText.text ="Chimneys help in pumping harmful and toxic gases far away from humans";
                     break;
            case 2 : ObjectiveHead.text = "Make Factory Eco-Friendly";
                     ObjectiveBody.text = "Build a waste pit\nBuild a purifier";
                     SubtitleText.text ="Waste recyclers help to reuse and recycle waste, thus reducing the amount of waste produced";
                     break;
            case 3 : ObjectiveHead.text = "Make Factory Eco-Friendly";
                     ObjectiveBody.text = "Build a purifier";
                     SubtitleText.text ="Waste pits are good for temporary handling of waste, and seperate solid waste from liquid waste";
                     break;
            case 4 : ObjectiveHead.text = "Purify factory waste";
                     ObjectiveBody.text = "Press F near purifier to purify river";
                     SubtitleText.text ="Great job! The waste is no longer being released into the river! Now, let's focus on cleaning the river.";
                     break;
            case 5 : ObjectiveHead.text = "Grow Crops";
                     ObjectiveBody.text = "Plough soil(0/4)";
                     SubtitleText.text ="The river is clean!\nLet's use this water to grow some crops. Start by ploughing the four fields";
                     break;
            case 6 : ObjectiveHead.text = "Grow Crops";
                     ObjectiveBody.text = "Plough soil(1/4)";
                     SubtitleText.text ="Keep ploughing!";
                     break;
            case 7 : ObjectiveHead.text = "Grow Crops";
                     ObjectiveBody.text = "Plough soil(2/4)";
                     SubtitleText.text ="Halfway there!";
                     break;
            case 8 : ObjectiveHead.text = "Grow Crops";
                     ObjectiveBody.text = "Plough soil(3/4)";
                     SubtitleText.text ="One left!";
                     break;
            case 9 : ObjectiveHead.text = "Grow Crops";
                     ObjectiveBody.text = "Connect the river to farms";
                     SubtitleText.text ="Great! Now, connect the river to these farms!";
                     break;
            case 10 : ObjectiveHead.text = "Grow Crops";
                     ObjectiveBody.text = "Sow Seeds(0/4)";
                     SubtitleText.text ="Excellent! We have access to fresh river water. \nAdd seeds now!";
                     break;
            case 11 : ObjectiveHead.text = "Grow Crops";
                     ObjectiveBody.text = "Sow Seeds(1/4)";
                     SubtitleText.text ="Three left!";
                     break;
            case 12 : ObjectiveHead.text = "Grow Crops";
                     ObjectiveBody.text = "Sow Seeds(2/4)";
                     SubtitleText.text ="Keep Going!";
                     break;
            case 13 : ObjectiveHead.text = "Grow Crops";
                     ObjectiveBody.text = "Sow Seeds(3/4)";
                     SubtitleText.text ="Last one!";
                     break;
            case 14 : ObjectiveHead.text = "Grow Crops";
                     ObjectiveBody.text = "Harvest the crops fully(4/4)";
                     SubtitleText.text ="All the plants are ready.You can now harvest your crops and sell them at the market!\nInteract with the field to harvest crops.";
                     break;
                     
            case 15 : ObjectiveHead.text = "Objective";
                     ObjectiveBody.text = "Objectives completed " +
                                          "Press T to Teleport";
                     SubtitleText.text ="Fantastic work! You're truly an outstanding farmer! I can't wait to taste your delicious fruits and vegetables!" +
                                        "Find the ruin stone to teleport";
                     break;
                     
                     
        }
    }

    private void Start()
    {
        StartCoroutine(ActivateHandsByTimeI());
        player1 = GameObject.FindGameObjectWithTag("Player");
        DynamicUI.enabled = false;
        MiniMapUI.enabled = false;
        InitialSetup.enabled = true;
        UpdateText();
    }

    private void Update()
    {
        if(isInitialDone){
            DynamicUI.enabled = true;
            MiniMapUI.enabled = true;
            InitialSetup.enabled = false;  
        }
        if(Input.GetKeyDown(KeyCode.Return) && !isInitialDone){
        DynamicUI.enabled = true;
        MiniMapUI.enabled = true;
        InitialSetup.enabled = false;
        isInitialDone = true;  
        }
        CheckPlayerInteractionI();
        CheckPlayerDamI();
    }

    private void CheckPlayerDamI()
    {
        if (Input.GetKeyDown(KeyCode.F) && !isMiniGameActive)
        {
            // Check proximity to the dam object
            if (IsPlayerNearRiver())
            {
                Debug.Log("Player pressed F near Dam");
                LoadDamSceneI();
                TextStatus++;
                UpdateText();
            }
        }
    }

    private IEnumerator ActivateHandsByTimeI()
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

    private void LoadDamSceneI()
    {
        isMiniGameActive = true;

        // Load the mini-game scene directly
        SceneManager.LoadScene(leverGameSceneName);
        Debug.Log("Mini-game scene loaded");
    }

    private void CheckPlayerInteractionI()
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
                    StartCoroutine(HandleInteractionI(handData));
                    TextStatus++;
                    UpdateText();
                }
            }
        }
    }

    private IEnumerator HandleInteractionI(HandData handData)
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

    private bool IsPlayerNearRiver()
    {
        // Calculate the distance between the player and the dam
        float distance = Vector3.Distance(player1.transform.position, transform.position);
        return distance <= proximityRange;
    }

    // public static int GetWater()
    // {
    //     return groundWater;
    // }
}

// New HandData class to store hand and tub transforms
// [System.Serializable]
// public class HandData
// {
//     public Transform input; // The hand transform
//     public List<Transform> Tubs; // List of tub transforms associated with this hand
// }
