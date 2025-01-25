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
    public AudioClip playAudioClip; // Add this to your class

    public AudioSource buttonClickAudioSource;
    public float fadeDuration = 7f;
    public float cameraTransitionTime = 10f;
    private Vector3 mainCameraStartPosition;

    [SerializeField] private Button quit;

    void Start()
    {   
        if(quit != null){
            quit.onClick.AddListener(()=>{
                Application.Quit();
            });
        }
        LevelMenuCanvas.gameObject.SetActive(false);
        mainCameraStartPosition = mainCamera.transform.position;
    }



    public void PlayButton_OnClick()
    {
        if (buttonClickAudioSource && playAudioClip)
        {
            buttonClickAudioSource.PlayOneShot(playAudioClip);
        }
        StartCoroutine(FadeAndSwitch("Play")); 
    }
    // public void BackButton_OnClick()
    // {
    //     // Play sound before starting transition
    //     if (buttonClickAudioSource && buttonClickSound)
    //     {
    //         buttonClickAudioSource.PlayOneShot(buttonClickSound);
    //     }
    //     StartCoroutine(FadeAndSwitch("Back")); 
    // }
    // public void PlayButton_OnClick()
    // {
    //     StartCoroutine(FadeAndSwitch("Play")); 
    // }
    //
    public void BackButton_OnClick()
    {
        StartCoroutine(FadeAndSwitch("Back")); 
    }

    IEnumerator FadeAndSwitch(string direction)
    {
        CanvasGroup mainMenuCanvasGroup = MainMenuCanvas.GetComponent<CanvasGroup>();
        CanvasGroup levelMenuCanvasGroup = LevelMenuCanvas.GetComponent<CanvasGroup>();

        float elapsedTime = 0f;
        Vector3 startPosition = mainCamera.transform.position;
        Vector3 targetPosition = (direction == "Play") ? new Vector3(4f, mainCamera.transform.position.y, 4f) : mainCameraStartPosition;

        while (elapsedTime < cameraTransitionTime)
        {
            // Ultra smooth camera movement with very slow initial progression
            float t = 1f - Mathf.Pow(1f - (elapsedTime / cameraTransitionTime), 3f);
            
            mainCamera.transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            
            if (direction == "Play")
            {
                mainMenuCanvasGroup.alpha = Mathf.SmoothStep(1f, 0f, t);
            }
            else
            {
                levelMenuCanvasGroup.alpha = Mathf.SmoothStep(1f, 0f, t);
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        mainCamera.transform.position = targetPosition;
        
        if (direction == "Play")
        {
            mainMenuCanvasGroup.alpha = 0f;
            LevelMenuCanvas.gameObject.SetActive(true);
            Plane.SetActive(false);
            Terrain.SetActive(false);
            
            levelMenuCanvasGroup.alpha = 0f;
            elapsedTime = 0f;
            while (elapsedTime < fadeDuration)
            {
                float t = Mathf.SmoothStep(0f, 1f, elapsedTime / fadeDuration);
                levelMenuCanvasGroup.alpha = t;
                elapsedTime += Time.deltaTime/2f;
                yield return null;
            }
            levelMenuCanvasGroup.alpha = 1f;
        }
        else 
        {
            levelMenuCanvasGroup.alpha = 0f;
            LevelMenuCanvas.gameObject.SetActive(false);
            Plane.SetActive(true);
            Terrain.SetActive(true);
            MainMenuCanvas.gameObject.SetActive(true);
            
            mainMenuCanvasGroup.alpha = 0f;
            elapsedTime = 0f;
            while (elapsedTime < fadeDuration)
            {
                float t = Mathf.SmoothStep(0f, 1f, elapsedTime / fadeDuration);
                mainMenuCanvasGroup.alpha = t;
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            mainMenuCanvasGroup.alpha = 1f;
        }
    }
}