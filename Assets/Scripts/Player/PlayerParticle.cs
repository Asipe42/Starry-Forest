using UnityEngine;

public class PlayerParticle : BaseParticle
{
    [SerializeField] ParticleSystem walkDust;
    [SerializeField] ParticleSystem slidingDust;
    [SerializeField] ParticleSystem takeItem;
    [SerializeField] ParticleSystem recover;

    void Update()
    {
        CheckWalkDust();
    }

    void CheckWalkDust()
    {
        SetParticleSystem(walkDust, PlayerController.instance.onGround && !PlayerController.instance.onPause, false);
    }

    public void PlaySlidingDust(bool state = true)
    {
        SetParticleSystem(slidingDust, state);
    }

    public void PlayTakeItem(bool state = true)
    {
        SetParticleSystem(takeItem, state);
    }

    public void PlayRecover(bool state = true)
    {
        SetParticleSystem(recover, state);
    }
}
