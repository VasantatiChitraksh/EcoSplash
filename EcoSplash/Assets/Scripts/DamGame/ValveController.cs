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
