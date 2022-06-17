using UnityEngine;

public struct UseKeys
{
    public static KeyCode jumpKey = KeyCode.X;
    public static KeyCode slidingKey = KeyCode.Z;
    public static KeyCode dashKey = KeyCode.RightArrow;
    public static KeyCode breakKey = KeyCode.LeftArrow;
    public static KeyCode specialKey = KeyCode.C;
}

public class InputManager : MonoBehaviour
{
    public static InputManager instance;

    public bool onLock;

    void Awake()
    {
        Initialize();
    }

    void Initialize()
    {
        instance = this;
    }

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
            if (PlayerController.instance.onJump && !PlayerController.instance.onGround)
                PlayerController.instance.Downhill();
            else
                PlayerController.instance.Jump();
        }

        if (Input.GetKeyDown(UseKeys.slidingKey))
        {
            PlayerController.instance.Sliding();
        }

        if (Input.GetKeyDown(UseKeys.dashKey))
        {
            PlayerController.instance.Dash();
        }

        if (Input.GetKeyDown(UseKeys.breakKey))
        {
            PlayerController.instance.Break();
        }

        if (Input.GetKeyDown(UseKeys.specialKey))
        {
            PlayerController.instance.PlaySpecialAction();
        }
    }
}
