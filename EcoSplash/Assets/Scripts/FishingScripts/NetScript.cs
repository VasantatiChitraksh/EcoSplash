using UnityEngine;

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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
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

                if (hit.collider != null && hit.collider.name == "Dustbin")
                {
                    isCaught = false;
                    Destroy(caughtObject);
                }
          
        }
        
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (isCaught)
        {
            return;
        }

        if(collision.gameObject.tag == "Garbage")
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
}
