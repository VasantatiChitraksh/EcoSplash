using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndMenu : MonoBehaviour
{
    [SerializeField] private Button menuButton;
    void Start()
    {
        if(menuButton != null){
            menuButton.onClick.AddListener(() => {
                SceneManager.LoadScene("HomeMenu");
            });
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
