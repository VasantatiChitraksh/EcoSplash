using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class LevelManagerDroughtThird : MonoBehaviour
{
    [Header("Hand and Object Data")]
    [SerializeField] private List<HandDataDroughtThird> handDataList; // List of hand data with transforms and tubs

    public static int groundWater = 0;

    [Header("Player Settings")]
    [SerializeField] private Transform player; // Player object
    [SerializeField] private float interactionDistance = 3f; // Distance within which the player can interact

    [Header("Hand Activation Settings")]
    [SerializeField] private float handActivationInterval = 15f; // Interval between hand activations
    private int currentHandIndex = 0; // Tracks the currently activated hand
    private bool isInteracting = false; // Prevents multiple interactions simultaneously

    public float proximityRange = 5f; // Range within which the player can interact
    public string secondPipesSceneName = "Gameplay_1"; // Name of the mini-game scene to load

    private GameObject player1;
    public bool isMiniGameActive = false; // To track if the mini-game is already active

    [SerializeField] private Transform miniGameActivator;

    private void Start()
    {
        StartCoroutine(ActivateHandsByTime());
        player1 = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        CheckPlayerInteraction();
        CheckPlayerSecondPipes();
    }

    private void CheckPlayerSecondPipes()
    {
        if (Input.GetKeyDown(KeyCode.F) && !isMiniGameActive)
        {
            // Check proximity to the dam object
            if (IsPlayerNearSecondPipes())
            {
                Debug.Log("Player pressed F near Dam");
                LoadSecondPipesScene();
            }
        }
    }

    private IEnumerator ActivateHandsByTime()
    {
        while (currentHandIndex < handDataList.Count)
        {
            HandDataDroughtThird handData = handDataList[currentHandIndex];

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

    private void LoadSecondPipesScene()
    {
        isMiniGameActive = true;

        // Load the mini-game scene directly
        SceneManager.LoadScene(secondPipesSceneName);
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
                }
            }
        }
    }

    private IEnumerator HandleInteraction(HandDataDroughtThird handData)
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

    private bool IsPlayerNearSecondPipes()
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

// New HandDataDroughtThird class to store hand and tub transforms
[System.Serializable]
public class HandDataDroughtThird
{
    public Transform input; // The hand transform
    public List<Transform> Tubs; // List of tub transforms associated with this hand
}
