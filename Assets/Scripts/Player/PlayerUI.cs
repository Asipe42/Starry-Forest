using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    public DandelionStack dandelionStack
    {
        get
        {
            if (dandelionStack != null)
                return dandelionStack;
            else
                return null;
        }

        private set
        {
            dandelionStack = value;
        }
    }

    public StrawberryStack strawberryStack
    {
        get
        {
            if (dandelionStack != null)
                return strawberryStack;
            else
                return null;
        }

        private set
        {
            strawberryStack = value;
        }
    }

    void Awake()
    {
        Initialize();
    }

    void Initialize()
    {
        dandelionStack = GameObject.FindObjectOfType<DandelionStack>();
        strawberryStack = GameObject.FindObjectOfType<StrawberryStack>();
    }
}