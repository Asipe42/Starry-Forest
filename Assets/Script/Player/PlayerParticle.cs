using UnityEngine;

public class PlayerParticle : BaseParticle
{
    [SerializeField] ParticleSystem dust;
    [SerializeField] ParticleSystem takeItem;

    public void PlayDust(bool state = true)
    {
        SetParticleSystem(dust, state);
    }

    public void PlayTakeItem(bool state = true)
    {
        SetParticleSystem(takeItem, state);
    }
}
