using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseParticle : MonoBehaviour
{
    protected void SetParticleSystem(ParticleSystem particleSystem, bool state)
    {
        if (state)
        {
            if (!particleSystem.isPlaying)
                particleSystem.Play();
        }
        else
        {
            particleSystem.Stop();
        }
    }
}
