using System.Collections;
using UnityEngine;

public class Result : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [SerializeField] GameObject health;
    [SerializeField] GameObject item;
    [SerializeField] GameObject currentrank;
    [SerializeField] GameObject finalRank;

    AudioClip popupClip;

    void Awake()
    {
        popupClip = Resources.Load<AudioClip>("Audio/SFX/SFX_Popup");
    }

    public IEnumerator ShowResult()
    {
        panel.SetActive(true);
        yield return new WaitForSeconds(1f);

        health.SetActive(true);
        AudioManager.instance.PlaySFX(popupClip);
        yield return new WaitForSeconds(1f);

        item.SetActive(true);
        AudioManager.instance.PlaySFX(popupClip);
        yield return new WaitForSeconds(1f);

        currentrank.SetActive(true);
        AudioManager.instance.PlaySFX(popupClip);
        yield return new WaitForSeconds(2f);
        
        health.SetActive(false);
        item.SetActive(false);
        currentrank.SetActive(false);

        AudioManager.instance.PlaySFX(popupClip);
        finalRank.SetActive(true);
    }
}
