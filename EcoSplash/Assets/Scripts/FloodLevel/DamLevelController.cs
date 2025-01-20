using UnityEngine;

public class DamLevelController : MonoBehaviour
{
    public float proximityRange = 5f; // Range within which the player can interact
    public string damSceneName = "DamScene"; // Name of the dam scene to load

    private GameObject player;

    void Start()
    {
        // Find the player object in the scene (ensure it has the "Player" tag)
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        // Check if the player presses the F key
        if (Input.GetKeyDown(KeyCode.F))
        {
            // Check proximity to the dam object
            if (IsPlayerNearDam())
            {
                // Load the dam scene using the Loader class
                Loader.Load(Loader.Scene.LeverGame); // Replace `GameScene` with the scene you want to load
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
