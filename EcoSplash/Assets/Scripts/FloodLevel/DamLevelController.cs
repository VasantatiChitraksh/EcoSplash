using UnityEngine;
using UnityEngine.SceneManagement;

public class DamLevelController : MonoBehaviour
{
    public float proximityRange = 5f; // Range within which the player can interact
    public string leverGameSceneName = "LeverGame"; // Name of the mini-game scene to load

    private GameObject player;
    private bool isMiniGameActive = false; // To track if the mini-game is already active

    void Start()
    {
        // Find the player object in the scene (ensure it has the "Player" tag)
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        // Check if the player presses the F key
        if (Input.GetKeyDown(KeyCode.F) && !isMiniGameActive)
        {
            // Check proximity to the dam object
            if (IsPlayerNearDam())
            {
                // Use the Loader class to load the mini-game
                Loader.Load(Loader.Scene.LeverGame); // Loading LeverGame scene
            }
        }
    }

    private bool IsPlayerNearDam()
    {
        // Calculate the distance between the player and the dam
        float distance = Vector3.Distance(player.transform.position, transform.position);
        return distance <= proximityRange;
    }
}
