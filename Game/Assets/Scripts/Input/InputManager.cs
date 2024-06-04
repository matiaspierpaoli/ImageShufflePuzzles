using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public event Action<Vector2> ClickEvent;

    public void OnClick(InputAction.CallbackContext context)
    {
        if (!context.started) return;

        Vector2 clickPosition = Mouse.current.position.ReadValue();
        
        ClickEvent?.Invoke(clickPosition);
    }
}
