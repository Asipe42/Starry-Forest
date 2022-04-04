using UnityEngine;

public class PlayerParticle : BaseParticle
{
    [SerializeField] ParticleSystem walkDust;
    [SerializeField] ParticleSystem slidingDust;
    [SerializeField] ParticleSystem takeItem;
    [SerializeField] ParticleSystem recover;

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

    void Update()
    {
        SetParticleSystem(walkDust, PlayerController.instance.onWalk, false);
    }
}
