using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UseKeys
{
    public static KeyCode jumpKey = KeyCode.Space;
    public static KeyCode SlidingKey = KeyCode.Z;
    public static KeyCode dashKey = KeyCode.Mouse0;
}

public class InputManager : MonoBehaviour
{
    public bool onLock;

    void Update()
    {
        if (!onLock)
        {
            InputKey();
        }
    }

    void InputKey()
    {
        if (Input.GetKeyDown(UseKeys.jumpKey))
        {
            if (PlayerController.instance.onJump)
                PlayerController.instance.Downhill();
            else
                PlayerController.instance.Jump();
        }

        if (Input.GetKeyDown(UseKeys.SlidingKey))
        {
            PlayerController.instance.Sliding();
        }

        if (Input.GetKeyDown(UseKeys.dashKey))
        {
            PlayerController.instance.Dash();
        }
    }
}
