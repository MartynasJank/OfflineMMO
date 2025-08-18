using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

/// <summary>
/// Configures InputSystemUIInputModule to support keyboard, mouse, and gamepad navigation.
/// Attach to an EventSystem object and assign an InputActionAsset with a UI action map.
/// </summary>
[RequireComponent(typeof(EventSystem))]
[RequireComponent(typeof(InputSystemUIInputModule))]
public class UIInputManager : MonoBehaviour
{
    [Tooltip("Input actions asset containing a 'UI' action map.")]
    public InputActionAsset actions;

    void Awake()
    {
        var module = GetComponent<InputSystemUIInputModule>();
        if (actions != null)
        {
            // Assign standard UI actions for point, click, navigate, submit and cancel
            module.actionsAsset = actions;
            module.point = InputActionReference.Create(actions.FindAction("UI/Point"));
            module.leftClick = InputActionReference.Create(actions.FindAction("UI/Click"));
            module.middleClick = InputActionReference.Create(actions.FindAction("UI/MiddleClick"));
            module.rightClick = InputActionReference.Create(actions.FindAction("UI/RightClick"));
            module.scrollWheel = InputActionReference.Create(actions.FindAction("UI/ScrollWheel"));
            module.move = InputActionReference.Create(actions.FindAction("UI/Navigate"));
            module.submit = InputActionReference.Create(actions.FindAction("UI/Submit"));
            module.cancel = InputActionReference.Create(actions.FindAction("UI/Cancel"));
        }
    }
}
