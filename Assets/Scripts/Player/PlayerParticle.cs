using UnityEngine;

public class PlayerParticle : BaseParticle
{
    [SerializeField] ParticleSystem walkDust;
    [SerializeField] ParticleSystem slidingDust;
    [SerializeField] ParticleSystem takeItem;
    [SerializeField] ParticleSystem recover;
    [SerializeField] ParticleSystem maxDash;
    [SerializeField] ParticleSystem dandelion;

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

    public void PlayDandelion(bool state = true)
    {
        SetParticleSystem(dandelion, state);
    }

    public void PlayMaxDash(bool state = true)
    {
        SetParticleSystem(maxDash, state);
    }
}
