using UnityEngine;

public class Activator : MonoBehaviour
{
    [SerializeField] bool[] activateChildren;

    void Start()
    {
        for (int i = 0; i < activateChildren.Length; i++)
        {
            transform.GetChild(i).gameObject.SetActive(activateChildren[i]);
        }
    }
}
