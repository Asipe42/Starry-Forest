using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    
    public Heart theHp;
    public Blood theBlood;

    void Awake()
    {
        instance = this;

        theHp = GameObject.FindObjectOfType<Heart>().GetComponent<Heart>();
        theBlood = GameObject.FindObjectOfType<Blood>().GetComponent<Blood>();
    }
}
