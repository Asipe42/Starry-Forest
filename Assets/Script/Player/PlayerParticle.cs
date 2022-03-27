using UnityEngine;

public class PlayerParticle : BaseParticle
{
    [SerializeField] ParticleSystem dust;

    public void PlayDust(bool state)
    {
        SetParticleSystem(dust, state);
    
    }
}
