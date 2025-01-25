using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleporter : MonoBehaviour
{
    public float proximityRange = 5f;
    private GameObject player1;// Flag to check if the player is nearby

    void Start()
    {
        player1 = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        // Check if the player is nearby and the T key is pressed
        if (IsPlayerNearDam() && Input.GetKeyDown(KeyCode.T))
        {
            SceneManager.LoadScene("UI");
            // Add additional functionality here
            Debug.Log("T key pressed. Action triggered!");
        }
    }
    
    private bool IsPlayerNearDam()
    {
        // Calculate the distance between the player and the dam
        float distance = Vector3.Distance(player1.transform.position, transform.position);
        return distance <= proximityRange;
    }
}