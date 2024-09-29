using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InputReader", menuName = "Game/Input Reader")]
public class InputReader : ScriptableObject, Input.IPlayerActions
{
    private Input inputActions;

    public event UnityAction<Vector2> MoveEvent;
    public event UnityAction MouseClickEvent;
    public event UnityAction MouseHoldEvent;
    public event UnityAction<float> MouseScrollEvent;

    private void OnEnable()
    {
        if (inputActions == null)
        {
            inputActions = new Input();
            inputActions.Player.SetCallbacks(this);
        }

        EnableInput();
    }

    private void OnDisable()
    {
        DisableInput();
    }
    /// <summary>
    /// Calling with a vector2
    /// </summary>
    public void OnWASD(InputAction.CallbackContext context)
    {
        Vector2 moveInput = context.ReadValue<Vector2>();
        MoveEvent?.Invoke(moveInput);
    }
    public void OnMouseClick(InputAction.CallbackContext context)
    {
        if(context.performed)
            MouseClickEvent?.Invoke();
    }
    public void OnMouseHold(InputAction.CallbackContext context)
    {
        if (context.performed)
            MouseHoldEvent?.Invoke();
    }
    public void OnMouseScroll(InputAction.CallbackContext context)
    {
        float input = context.ReadValue<Vector2>().y;
        MouseScrollEvent?.Invoke(-input);
    }

    private void EnableInput()
    {
        inputActions.Player.Enable();
    }

    private void DisableInput()
    {
        inputActions.Player.Disable();
    }
}