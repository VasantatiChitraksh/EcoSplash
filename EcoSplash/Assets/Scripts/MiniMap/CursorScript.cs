using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public Texture2D defaultCursor;
    public Texture2D hoverCursor;
    private Vector2 hotSpot;
    void Start()
    {
        // Make the cursor visible and unlock it
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        SetCursor(defaultCursor);
        hotSpot = new Vector2(defaultCursor.width / 2, defaultCursor.height / 2);

    }

    public void SetCursor(Texture2D cursorTexture)
    {
        Cursor.SetCursor(cursorTexture, hotSpot, CursorMode.Auto);
    }

    public void OnMouseEnter()
    {
        SetCursor(hoverCursor);
    }

    public void OnMouseExit()
    {
        SetCursor(defaultCursor);
    }
}
