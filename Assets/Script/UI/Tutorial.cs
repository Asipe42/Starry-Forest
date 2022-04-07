using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class Tutorial : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] AudioClip showClip;

    void Awake()
    {
        showClip = Resources.Load<AudioClip>("Audio/SFX/SFX_Show");
    }

    void Start()
    {
        StartCoroutine(Show(2f));
    }

    IEnumerator Show(float delay)
    {
        yield return new WaitForSeconds(delay);

        AudioManager.instance.PlaySFX(showClip, 0f, 0.8f);
        image.DOColor(new Color(0f, 0f, 0f, 0.4f), 0.5f);
        text.DOColor(Color.white, 0.5f);

        StartCoroutine(Hide(3f));
    }

    IEnumerator Hide(float delay)
    {
        yield return new WaitForSeconds(delay);

        image.DOColor(Color.clear, 1.0f);
        text.DOColor(Color.clear, 1f);
    }
}
