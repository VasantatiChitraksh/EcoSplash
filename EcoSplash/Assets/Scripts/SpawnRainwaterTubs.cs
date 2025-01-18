using UnityEngine;
using System.Collections.Generic;

public class SpawnRainwaterTubs : MonoBehaviour
{
    [SerializeField] private List<GameObject> list_of_hands; // List of hand objects
    [SerializeField] private GameObject tubPrefab;           // Tub prefab

    private int handIndex = -1;         // Keeps track of the current hand
    private int interactionCount = 0;  // Counts interactions for the current hand

    void Start()
    {
        // Deactivate all hands initially
        foreach (GameObject hand in list_of_hands)
        {
            hand.SetActive(false);
        }
    }

    void Update()
    {
        // Activate the first hand after 5 seconds
        if (Time.time > 5f && handIndex == -1)
        {
            handIndex = 0;
            list_of_hands[handIndex].SetActive(true);
        }

        // Handle interactions with the active hand
        if (handIndex != -1 && Input.GetKeyDown(KeyCode.E))
        {
            interactionCount++;

            if (interactionCount <= 2)
            {
                // Spawn a tub below the hand's position
                SpawnTubForHand(handIndex);
            }

            if (interactionCount == 2)
            {
                // Deactivate the hand after the second interaction
                list_of_hands[handIndex].SetActive(false);

                // Move to the next hand, if available
                handIndex++;
                interactionCount = 0;

                if (handIndex < list_of_hands.Count)
                {
                    list_of_hands[handIndex].SetActive(true);
                }
                else
                {
                    handIndex = -1; // No more hands to activate
                }
            }
        }
    }

    void SpawnTubForHand(int index)
    {
        if (index >= 0 && index < list_of_hands.Count)
        {
            // Calculate spawn position below the hand with an offset of 4
            Vector3 spawnPosition = list_of_hands[index].transform.position + new Vector3(0, -4, 0);

            // Instantiate the tub at the calculated position
            Instantiate(tubPrefab, spawnPosition, Quaternion.identity);
        }
    }
}
