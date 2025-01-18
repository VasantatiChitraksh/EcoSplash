using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }
    private InputSystem_Actions playerInputActions;

    private void Awake()
    {
        playerInputActions = new InputSystem_Actions();
        playerInputActions.Player.Enable();
        Instance = this;
    }

    private void OnDestroy()
    {
        playerInputActions.Dispose();
    }

    public Vector2 GetMovementVectorNormalized()
    {
        if (playerInputActions == null)
        {
            Debug.LogError("PlayerInputActions is not initialized.");
            return Vector2.zero;
        }

        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();
        return inputVector.normalized;
    }
}
