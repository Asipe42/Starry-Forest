using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Result : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [SerializeField] GameObject health;
    [SerializeField] GameObject item;
    [SerializeField] GameObject currentrank;
    [SerializeField] GameObject finalRank;

    List<GameObject> resultItem;

    AudioClip popupClip;

    void Awake()
    {
        popupClip = Resources.Load<AudioClip>("Audio/SFX/SFX_Popup");

        resultItem = new List<GameObject>();
    }

    void Start()
    {
        resultItem.Add(health);
        resultItem.Add(item);
        resultItem.Add(currentrank);
        resultItem.Add(finalRank);
    }

    public IEnumerator ShowResult()
    {
        for (int i = 0; i < resultItem.Count; i++)
        {
            if (resultItem[i] == finalRank)
            {
                for (int j = i; j >= 0; j--)
                {
                    resultItem[j].SetActive(false);
                    resultItem[j].SetActive(false);
                    resultItem[j].SetActive(false);
                }
                yield return new WaitForSeconds(1f);
            }

            ActivateGameObject(resultItem[i]);
            yield return new WaitForSeconds(1f);
        }
    }

    void ActivateGameObject(GameObject go)
    {
        go.SetActive(true);
        SFXController.instance.PlaySFX(popupClip);
    }
}
