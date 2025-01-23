using UnityEngine;
using UnityEngine.SceneManagement;

public class DrillerMovement : MonoBehaviour
{
    private float downSpeed = 2f;
    private float sidewaysSpeed = 4f;
    private float moveDistance = 1f;
    private Vector3 lastPosition;

    Camera cam;
    public GameObject prefab; 
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = Camera.main;
        lastPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.y < -75f)
        {
            SceneManager.LoadScene("DroughtSceneWell");
        }

        if(Vector3.Distance(lastPosition, transform.position) >= moveDistance)
        {
            Dig();
        }

        FollowPlayer();

        Rigidbody2D rb = GetComponent<Rigidbody2D>(); 
        rb.linearVelocity = new Vector2(0, -downSpeed);

        float clampedX = Mathf.Clamp(transform.position.x, -10f, 10f);
        transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);

        if (Input.GetMouseButton(0))
        {
            FollowMouse();
        } 
    }

    private void FollowMouse()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = -2;  
        mousePos.y = transform.position.y;

        transform.position = Vector3.MoveTowards(transform.position, mousePos, sidewaysSpeed * Time.deltaTime);
    }

    private void FollowPlayer()
    {
        cam.transform.position = new Vector3(transform.position.x, transform.position.y, cam.transform.position.z);
    }

    private void Dig()
    {
        Instantiate(prefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
        lastPosition = transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.parent.tag == "Garbage")
        {
            gameObject.SetActive(false);
        }
    }
}
