using UnityEngine;

public class CursorManager : MonoBehaviour
{
    void Start()
    {
        // Make the cursor visible and unlock it
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
