// using UnityEngine;
// using UnityEngine.UI;

// public class ValveController : MonoBehaviour
// {
//     public Slider tensionSlider;
//     public Slider progressBar;
//     public RectTransform idealZoneRect;
//     public float rotationSpeed = 100f;
//     public float tensionIncreaseRate = 1f;
//     public float tensionDecreaseRate = 0.5f;
//     public float progressIncreaseRate = 2f;
//     public float progressDecreaseRate = 0f;
//     public float idealZoneMin = 0.375f;
//     public float idealZoneMax = 0.625f;
//     private Quaternion initialRotation;
//     private bool isRotating = false;
//     private float progressMaxValue = 0.15f; 
//     public Transform upstreamWater;
//     public Transform lowstreamWater;
//     public float waterLevelChangeRate = 0.2f;

//     public bool maxProgressReached = false;
//     public AudioSource audioSource;

//     void Start()
//     {
//         initialRotation = transform.localRotation;
//         progressBar.maxValue = progressMaxValue; 
//     }

//     void Update()
//     {
//         // Valve Rotation and Tension Control
//         if (Input.GetKeyDown(KeyCode.Space) && !isRotating)
//         {
//             isRotating = true;
//         }

//         if (Input.GetKey(KeyCode.Space) && isRotating)
//         {
//             float currentZRotation = transform.localEulerAngles.z;
//             transform.localRotation = Quaternion.Euler(0f, 0f, currentZRotation - rotationSpeed * Time.deltaTime);
//             tensionSlider.value = Mathf.Clamp01(tensionSlider.value + tensionIncreaseRate * Time.deltaTime);
//         }
//         else if (isRotating)
//         {
//             if (tensionSlider.value > 0)
//             {
//                 float currentZRotation = transform.localEulerAngles.z;
//                 transform.localRotation = Quaternion.Euler(0f, 0f, currentZRotation + rotationSpeed * Time.deltaTime);
//                 tensionSlider.value = Mathf.Clamp01(tensionSlider.value - tensionDecreaseRate * Time.deltaTime);
//             }
//             else
//             {
//                 transform.localRotation = initialRotation;
//                 isRotating = false;
//             }
//         }

//         // Progress Bar Logic
//         float tensionValue = tensionSlider.value;

//         if (tensionValue >= idealZoneMin && tensionValue <= idealZoneMax)
//         {
//             progressBar.value = Mathf.Clamp(progressBar.value + progressIncreaseRate * Time.deltaTime, 0f, progressMaxValue); // Clamped to new max
//         }
//         else
//         {
//             // progressBar.value = Mathf.Clamp(progressBar.value - progressDecreaseRate * Time.deltaTime, 0f, progressMaxValue); // Clamped to new max
//         }
//         if (progressBar.value >= progressBar.maxValue)
//         {
//             Debug.Log("Progress reached maximum value!");
//             maxProgressReached = true;
//         }
//         // Water Level Logic
//         float progressNormalized = progressBar.value / progressBar.maxValue; // Normalize progress to 0-1 range
//         float waterLevelChange = progressNormalized * waterLevelChangeRate; // Calculate the change in water level

//         // Decrease Upstream Water Y Position
//         if(!maxProgressReached){
//         upstreamWater.position = new Vector3(upstreamWater.position.x, upstreamWater.position.y - waterLevelChange * (Time.deltaTime/10f), upstreamWater.position.z);
        
//         // Increase Lowstream Water Y Position. If you want it to decrease as well, use the same logic as upstream
//         lowstreamWater.position = new Vector3(lowstreamWater.position.x, lowstreamWater.position.y + waterLevelChange * Time.deltaTime, lowstreamWater.position.z);
//         }
//     }
// }
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ValveController : MonoBehaviour
{
    public Slider tensionSlider;
    public Slider progressBar;
    public RectTransform idealZoneRect;
    public float rotationSpeed = 100f;
    public float tensionIncreaseRate = 1f;
    public float tensionDecreaseRate = 0.5f;
    public float progressIncreaseRate = 2f;
    public float progressDecreaseRate = 0f;
    public float idealZoneMin = 0.375f;
    public float idealZoneMax = 0.625f;
    private Quaternion initialRotation;
    private bool isRotating = false;
    private float progressMaxValue = 0.15f;
    public Transform upstreamWater;
    public Transform lowstreamWater;
    public float waterLevelChangeRate = 0.2f;

    public bool maxProgressReached = false;
    public AudioSource audioSource; // Reference to the AudioSource component

    public GameObject endScreen;

    public GameObject UItext;

    void Start()
    {
        initialRotation = transform.localRotation;
        progressBar.maxValue = progressMaxValue;
    }

    void Update()
    {
        HandleValveRotation();
        HandleWaterLevels();
    }

    private void HandleValveRotation()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isRotating)
        {
            isRotating = true;
            
            // Play audio when space is pressed
            if (audioSource != null && !audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }

        if (Input.GetKey(KeyCode.Space) && isRotating)
        {
            float currentZRotation = transform.localEulerAngles.z;
            transform.localRotation = Quaternion.Euler(0f, 0f, currentZRotation - rotationSpeed * Time.deltaTime);
            tensionSlider.value = Mathf.Clamp01(tensionSlider.value + tensionIncreaseRate * Time.deltaTime);
        }
        else if (isRotating)
        {
            if (tensionSlider.value > 0)
            {
                float currentZRotation = transform.localEulerAngles.z;
                transform.localRotation = Quaternion.Euler(0f, 0f, currentZRotation + rotationSpeed * Time.deltaTime);
                tensionSlider.value = Mathf.Clamp01(tensionSlider.value - tensionDecreaseRate * Time.deltaTime);
            }
            else
            {
                transform.localRotation = initialRotation;
                isRotating = false;

                // Stop audio when rotation stops
                if (audioSource != null && audioSource.isPlaying)
                {
                    audioSource.Stop();
                }
            }
        }

        if (tensionSlider.value >= idealZoneMin && tensionSlider.value <= idealZoneMax)
        {
            progressBar.value = Mathf.Clamp(progressBar.value + progressIncreaseRate * Time.deltaTime, 0f, progressMaxValue);
        }

        if (progressBar.value >= progressBar.maxValue && !maxProgressReached)
        {
            Debug.Log("Progress reached maximum value!");
            maxProgressReached = true;
            OnMaxProgressReached();
        }
    }

    private void HandleWaterLevels()
    {
        if (!maxProgressReached)
        {
            float progressNormalized = progressBar.value / progressBar.maxValue;
            float waterLevelChange = progressNormalized * waterLevelChangeRate;

            upstreamWater.position -= new Vector3(0, waterLevelChange * Time.deltaTime, 0);
            lowstreamWater.position += new Vector3(0, waterLevelChange * Time.deltaTime, 0);
        }
    }

    private void OnMaxProgressReached()
    {
        Debug.Log("Mini-game completed! Loading the next scene...");
        SceneManager.LoadScene("FloodSceneFull");

        if (LevelManagerFull.Instance != null)
        {
            LevelManagerFull.Instance.isMiniGameActive = false;
            LevelManagerFull.Instance.NotifyMiniGameCompleted();
        }
    }
}
