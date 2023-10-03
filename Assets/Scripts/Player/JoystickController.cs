using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class JoystickController : MonoBehaviour
{
    [SerializeField] private VariableJoystick variableJoystick;
    [SerializeField] private bool isGamepadRightStick;
    private bool isEnabled = false;
    public event Action<Vector2> MovementOccured;
    private Vector2 lastMovementDirection = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        EnableJoystickInput();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isEnabled) return;

        Vector2 movementDirection = new Vector2(variableJoystick.Direction.x, variableJoystick.Direction.y);

        if (isGamepadRightStick)
        {
            if (lastMovementDirection != movementDirection)
            {
                InputSystem.QueueStateEvent(Gamepad.current, new GamepadState() { rightStick = movementDirection });
                lastMovementDirection = movementDirection;
            }
        }
        else
        {
            MovementOccured?.Invoke(movementDirection);
        }
    }

    public void EnableJoystickInput()
    {
        isEnabled = true;
        // inputCanvas.gameObject.SetActive(true);
    }
}
