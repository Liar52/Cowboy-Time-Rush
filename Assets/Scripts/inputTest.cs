using UnityEngine;
using UnityEngine.InputSystem;

public class InputTest : MonoBehaviour
{
    public InputActionProperty testActionValue;
    public InputActionProperty testActionButton;

    void Start()
    {
        testActionValue.action.Enable();
        testActionButton.action.Enable();
    }

    void OnDestroy()
    {
        testActionValue.action.Disable();
        testActionButton.action.Disable();
    }

    void Update()
    {
        float value = testActionValue.action.ReadValue<float>();
        Debug.Log("VALUE : " + value);

        bool button = testActionButton.action.IsPressed();
        Debug.Log("BUTTON : " + button);
    }
}