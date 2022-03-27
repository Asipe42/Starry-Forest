using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode SlidingKey = KeyCode.Z;

    void Update()
    {
        InputKey();   
    }

    void InputKey()
    {
        if (Input.GetKeyDown(jumpKey))
        {
            if (PlayerController.instance.onJump)
                PlayerController.instance.Downhill();
            else
                PlayerController.instance.Jump();
        }

        if (Input.GetKeyDown(SlidingKey))
        {
            PlayerController.instance.Sliding();
        }
    }
}
