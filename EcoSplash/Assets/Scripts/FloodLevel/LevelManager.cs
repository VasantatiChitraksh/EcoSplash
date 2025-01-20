using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    [Header("Hand and Object Data")]
    [SerializeField] private List<HandData> handDataList; // List of hand data with transforms and tubs

    [Header("Player Settings")]
    [SerializeField] private Transform player; // Player object
    [SerializeField] private float interactionDistance = 3f; // Distance within which the player can interact

    [Header("Hand Activation Settings")]
    [SerializeField] private float handActivationInterval = 15f; // Interval between hand activations
    private int currentHandIndex = 0; // Tracks the currently activated hand
    private bool isInteracting = false; // Prevents multiple interactions simultaneously

    private void Start()
    {
        StartCoroutine(ActivateHandsByTime());
    }

    private void Update()
    {
        CheckPlayerInteraction();
    }

    private IEnumerator ActivateHandsByTime()
    {
        while (currentHandIndex < handDataList.Count)
        {
            HandData handData = handDataList[currentHandIndex];

            // Check if the hand's Transform is not null and not already active
            if (handData.input != null && !handData.input.gameObject.activeSelf)
            {   
                // Activate the hand at its predefined position and rotation
                Debug.Log(handData.input.position);
                handData.input.gameObject.SetActive(true);
                Debug.Log(handData.input.gameObject);

                Debug.Log("Hand Activated: " + handData.input.name);
            }

            // Wait until the current hand is deactivated (interaction completed)
            yield return new WaitUntil(() => !handData.input.gameObject.activeSelf); 

            currentHandIndex++;

            // Wait for the specified interval before activating the next hand
            yield return new WaitForSeconds(handActivationInterval);
        }

        Debug.Log("All hands have been activated based on time.");
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

    private IEnumerator HandleInteraction(HandData handData)
    {
        isInteracting = true;

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
}

// New HandData class to store hand and tub transforms
[System.Serializable]
public class HandData
{
    public Transform input; // The hand transform
    public List<Transform> Tubs; // List of tub transforms associated with this hand
}
