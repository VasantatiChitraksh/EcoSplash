using UnityEngine;

public class FloodSim : MonoBehaviour
{
    [SerializeField] Transform lake;
    [SerializeField] float speed = 0.01f;
    float startingY;
    void Start()
    {
        startingY = lake.position.y;
    }


    void Update()
    {
        float newY = startingY + speed * Time.time;
        lake.position = new Vector3(lake.position.x, newY, lake.position.z);
    }
}
