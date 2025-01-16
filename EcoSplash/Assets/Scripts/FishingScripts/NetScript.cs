using UnityEngine;

public class NetScript : MonoBehaviour
{
    private float minX = -12f;
    private float maxX = 11f;
    private float minY = -7f;
    private float maxY = -3f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Input.mousePosition;

        mousePos = Camera.main.ScreenToWorldPoint(mousePos);

        mousePos.z = transform.position.z;
        mousePos.x = Mathf.Clamp(mousePos.x, minX, maxX);
        mousePos.y = Mathf.Clamp(mousePos.y, minY, maxY);
        transform.position = mousePos;

        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Garbage")
        {
            collision.gameObject.SetActive(false);
        }
    }
}
