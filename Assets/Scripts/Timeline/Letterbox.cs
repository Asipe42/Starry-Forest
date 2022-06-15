using UnityEngine;
using DG.Tweening;

public class Letterbox : MonoBehaviour
{
    [SerializeField] DOTweenAnimation anim;
    [SerializeField] string[] ID;

    public void ShowLetterBox(bool state)
    {
        if (state)
        {
            anim.DOPlayAllById(ID[0]);
        }
        else
        {
            anim.DOPlayAllById(ID[1]);
        }
    }
}