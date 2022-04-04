using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseParticle : MonoBehaviour
{
    protected void SetParticleSystem(ParticleSystem particleSystem, bool state, bool overlap = true)
    {
        if (overlap)
        {
            if (state)
            {
                particleSystem.Play();
            }
            else
            {
                particleSystem.Stop();
            }
        }
        else
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
}
