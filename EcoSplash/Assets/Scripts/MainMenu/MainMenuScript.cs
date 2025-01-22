// using UnityEngine;
// using UnityEngine.UI;
// using System.Collections;

// public class MenuManager : MonoBehaviour
// {
//     public Canvas MainMenuCanvas;
//     public Canvas LevelMenuCanvas;
//     public GameObject Plane;
//     public GameObject Terrain;
//     public Camera mainCamera; 

//     public float fadeDuration = 1f; // Duration of fade animation
//     public float cameraTransitionTime = 1f; // Duration of camera transition

//     void Start()
//     {
//         LevelMenuCanvas.gameObject.SetActive(false); 
//     }

//     public void PlayButton_OnClick()
//     {
//         StartCoroutine(FadeAndSwitch("Play")); 
//     }

//     public void BackButton_OnClick()
//     {
//         StartCoroutine(FadeAndSwitch("Back")); 
//     }

//     IEnumerator FadeAndSwitch(string direction)
//     {
//         // Get CanvasGroup components
//         CanvasGroup mainMenuCanvasGroup = MainMenuCanvas.GetComponent<CanvasGroup>();
//         CanvasGroup levelMenuCanvasGroup = LevelMenuCanvas.GetComponent<CanvasGroup>();

//         // Camera Transition
//         float elapsedTime = 0f;
//         float startX = mainCamera.transform.position.x;
//         float startZ = mainCamera.transform.position.z;

//         while (elapsedTime < cameraTransitionTime)
//         {
//             float t = elapsedTime / cameraTransitionTime; 
//             float newX = Mathf.Lerp(startX, 2f, t); 
//             float newZ = Mathf.Lerp(startZ, 2f, t); 

//             mainCamera.transform.position = new Vector3(newX, 
//                                                       mainCamera.transform.position.y, 
//                                                       newZ);

//             elapsedTime += Time.deltaTime;
//             yield return null;
//         }

//         // Fade out MainMenuCanvas or LevelMenuCanvas
//         if (direction == "Play")
//         {
//             mainMenuCanvasGroup.alpha = 1f; 
//         }
//         else if (direction == "Back") 
//         {
//             levelMenuCanvasGroup.alpha = 1f; 
//         }

//         elapsedTime = 0f;

//         while (elapsedTime < fadeDuration)
//         {
//             if (direction == "Play")
//             {
//                 float alpha = 1f - (elapsedTime / fadeDuration); 
//                 mainMenuCanvasGroup.alpha = alpha;
//             }
//             else if (direction == "Back")
//             {
//                 float alpha = 1f - (elapsedTime / fadeDuration); 
//                 levelMenuCanvasGroup.alpha = alpha;
//             }

//             elapsedTime += Time.deltaTime;
//             yield return null;
//         }

//         // Toggle Canvas and Plane/Terrain visibility
//         if (direction == "Play")
//         {
//             LevelMenuCanvas.gameObject.SetActive(true);
//             Plane.SetActive(false);
//             Terrain.SetActive(false);
//         }
//         else 
//         {
//             LevelMenuCanvas.gameObject.SetActive(false);
//             Plane.SetActive(true);
//             Terrain.SetActive(true);
//             MainMenuCanvas.gameObject.SetActive(true); // Set MainMenuCanvas active on Back
//         }

//         // Fade in LevelMenuCanvas or MainMenuCanvas
//         if (direction == "Play")
//         {
//             levelMenuCanvasGroup.alpha = 0f; 
//         }
//         else if (direction == "Back")
//         {
//             mainMenuCanvasGroup.alpha = 0f; 
//         }

//         elapsedTime = 0f;

//         while (elapsedTime < fadeDuration)
//         {
//             if (direction == "Play")
//             {
//                 float alpha = elapsedTime / fadeDuration; 
//                 levelMenuCanvasGroup.alpha = alpha;
//             }
//             else if (direction == "Back")
//             {
//                 float alpha = elapsedTime / fadeDuration; 
//                 mainMenuCanvasGroup.alpha = alpha;
//             }

//             elapsedTime += Time.deltaTime;
//             yield return null;
//         }
//     }
// }

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuManager : MonoBehaviour
{
    public Canvas MainMenuCanvas;
    public Canvas LevelMenuCanvas;
    public GameObject Plane;
    public GameObject Terrain;
    public Camera mainCamera; 

    public float fadeDuration = 1f; // Duration of fade animation
    public float cameraTransitionTime = 1f; // Duration of camera transition
    public Vector3 mainCameraStartPosition = Vector3.zero; // Store the initial camera position

    void Start()
    {
        LevelMenuCanvas.gameObject.SetActive(false);
        mainCameraStartPosition = mainCamera.transform.position; // Store initial position
    }

    public void PlayButton_OnClick()
    {
        StartCoroutine(FadeAndSwitch("Play")); 
    }

    public void BackButton_OnClick()
    {
        StartCoroutine(FadeAndSwitch("Back")); 
    }

    IEnumerator FadeAndSwitch(string direction)
    {
        // Get CanvasGroup components
        CanvasGroup mainMenuCanvasGroup = MainMenuCanvas.GetComponent<CanvasGroup>();
        CanvasGroup levelMenuCanvasGroup = LevelMenuCanvas.GetComponent<CanvasGroup>();

        // Camera Transition
        float elapsedTime = 0f;
        Vector3 startPosition = mainCamera.transform.position;
        Vector3 targetPosition = (direction == "Play") ? new Vector3(4f, mainCamera.transform.position.y, 4f) : mainCameraStartPosition; 

        while (elapsedTime < cameraTransitionTime)
        {
            float t = elapsedTime / cameraTransitionTime; 
            mainCamera.transform.position = Vector3.Lerp(startPosition, targetPosition, t); 
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Fade out MainMenuCanvas or LevelMenuCanvas
        if (direction == "Play")
        {
            mainMenuCanvasGroup.alpha = 1f; 
        }
        else if (direction == "Back") 
        {
            levelMenuCanvasGroup.alpha = 1f; 
        }

        elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            if (direction == "Play")
            {
                float alpha = 1f - (elapsedTime / fadeDuration); 
                mainMenuCanvasGroup.alpha = alpha;
            }
            else if (direction == "Back")
            {
                float alpha = 1f - (elapsedTime / fadeDuration); 
                levelMenuCanvasGroup.alpha = alpha;
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Toggle Canvas and Plane/Terrain visibility
        if (direction == "Play")
        {
            LevelMenuCanvas.gameObject.SetActive(true);
            Plane.SetActive(false);
            Terrain.SetActive(false);
        }
        else 
        {
            LevelMenuCanvas.gameObject.SetActive(false);
            Plane.SetActive(true);
            Terrain.SetActive(true);
            MainMenuCanvas.gameObject.SetActive(true); // Set MainMenuCanvas active on Back
        }

        // Fade in LevelMenuCanvas or MainMenuCanvas
        if (direction == "Play")
        {
            levelMenuCanvasGroup.alpha = 0f; 
        }
        else if (direction == "Back")
        {
            mainMenuCanvasGroup.alpha = 0f; 
        }

        elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            if (direction == "Play")
            {
                float alpha = elapsedTime / fadeDuration; 
                levelMenuCanvasGroup.alpha = alpha;
            }
            else if (direction == "Back")
            {
                float alpha = elapsedTime / fadeDuration; 
                mainMenuCanvasGroup.alpha = alpha;
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}