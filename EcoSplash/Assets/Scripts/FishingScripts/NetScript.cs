using UnityEngine;
using UnityEngine.SceneManagement; // Required for scene management

public class NetScript : MonoBehaviour
{
    private float minX = -12f;
    private float maxX = 11f;
    private float minY = -7f;
    private float maxY = 3f;

    private float netX = -2f;
    private float netY = -2f;

    private bool isCaught = false;

    GameObject caughtObject;

    private int collectedGarbage = 0; // Counter for collected garbage objects
    private int requiredGarbage = 6; // Number of objects required to load the next scene

    void Start()
    {
        // Initialize variables or perform any setup if needed
    }

    void Update()
    {
        followMouse();

        if (isCaught)
        {
            caughtObject.GetComponent<Transform>().position = this.transform.position + new Vector3(netX, netY, 1);
        }

        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

            if (isCaught && hit.collider != null && hit.collider.name == "Dustbin")
            {
                isCaught = false;

                // Increment collected garbage count and destroy the caught object
                Destroy(caughtObject);
                collectedGarbage++;

                // Check if the required number of garbage objects has been collected
                if (collectedGarbage >= requiredGarbage)
                {
                    LoadNextScene();
                }
            }
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (isCaught)
        {
            return;
        }

        if (collision.gameObject.tag == "Garbage")
        {
            caughtObject = collision.gameObject;
            Debug.Log("Collision detected");
            isCaught = true;
        }
    }

    private void followMouse()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);

        mousePos.z = transform.position.z;
        mousePos.x = Mathf.Clamp(mousePos.x, minX, maxX);
        mousePos.y = Mathf.Clamp(mousePos.y, minY, maxY);
        transform.position = mousePos;
    }

    private void LoadNextScene()
    {
        Debug.Log("Required objects collected! Loading next scene...");
        // Replace "NextSceneName" with the name of your scene
        SceneManager.LoadScene("IndustrialLevelEnd");
    }
}
