using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float playerSpeed = 2f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("a"))
        {
            transform.position = transform.position - new Vector3(playerSpeed,0,0);
        }

        if (Input.GetKey("d"))
        {
            transform.position = transform.position + new Vector3(playerSpeed,0,0);
        }
    }
}
