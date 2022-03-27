using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [SerializeField] AudioClip walkClip;
    [SerializeField] AudioClip jumpClip;
    [SerializeField] AudioClip downhillClip;
    [SerializeField] AudioClip slidingClip;

    AudioSource audioSource; // walk

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySFX_Walk()
    {

    }

    public void PlaySFX_Jump(float delay)
    {
        AudioManager.instance.PlaySFX(jumpClip, delay);
    }

    public void PlaySFX_Downhill(float delay)
    {
        AudioManager.instance.PlaySFX(downhillClip, delay);
    }

    public void PlaySFX_Sliding(float delay)
    {
        AudioManager.instance.PlaySFX(slidingClip, delay);
    }
}
