using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Interactor : MonoBehaviour
{
    public void ClickTile(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

    }
}
