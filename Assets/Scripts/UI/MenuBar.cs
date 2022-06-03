using System.Collections;
using UnityEngine;

public class MenuBar : MonoBehaviour
{
    [SerializeField] float moveSpeed = 20f;
    [SerializeField] AudioClip clip;

    Vector3 destination;

    AudioSource audioSource;

    void Awake()
    {
        Initialize();
    }

    #region Initial Setting
    void Initialize()
    {
        audioSource = GetComponent<AudioSource>();
    }
    #endregion

    /// <summary>
    /// Title 씬의 메뉴 바를 움직입니다.
    /// </summary>
    /// <param name="destination"></param>
    /// <returns></returns>
    public IEnumerator MoveBar(Vector3 destination)
    {
        this.destination = destination;
        audioSource.PlayOneShot(clip);

        while (transform.localPosition.y <= this.destination.y - 0.05 || transform.localPosition.y >= this.destination.y + 0.05)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, this.destination, Time.deltaTime * moveSpeed);

            yield return null;
        }
    }
}
