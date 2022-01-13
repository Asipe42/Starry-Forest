using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVFX : MonoBehaviour
{
    [Header("Particles")]
    [SerializeField] ParticleSystem _dustEffect;
    [SerializeField] ParticleSystem _takeItemEffect;
    [SerializeField] ParticleSystem _recoverEffect;
    [SerializeField] ParticleSystem _dandelionEffect;
    [SerializeField] ParticleSystem _dashEffect;
    [SerializeField] ParticleSystem _knockdownEffect;

    public void PlayVFX(string clip)
    {
        switch (clip)
        {
            case Definition.VFX_DUST:
                if (!_dustEffect.isPlaying)
                    _dustEffect.Play();
                break;
            case Definition.VFX_TAKE_ITEM:
                    _takeItemEffect.Play();
                break;
            case Definition.VFX_RECOVER:
                if (!_recoverEffect.isPlaying)
                    _recoverEffect.Play();
                break;
            case Definition.VFX_DANDELION:
                if (!_dandelionEffect.isPlaying)
                _dandelionEffect.Play();
                break;
            case Definition.VFX_DASH:
                if (!_dashEffect.isPlaying)
                    _dashEffect.Play();
                break;
            case Definition.VFX_KNOCKDOWN:
                if (!_knockdownEffect.isPlaying)
                    _knockdownEffect.Play();
                break;
        }
    }

    public void StopVFX(string clip)
    {
        switch (clip)
        {
            case Definition.VFX_DUST:
                _dustEffect.Stop();
                break;
            case Definition.VFX_TAKE_ITEM:
                _takeItemEffect.Stop();
                break;
            case Definition.VFX_RECOVER:
                _recoverEffect.Stop();
                break;
            case Definition.VFX_DANDELION:
                _dandelionEffect.Stop();
                break;
            case Definition.VFX_DASH:
                _dashEffect.Stop();
                break;
            case Definition.VFX_KNOCKDOWN:
                _knockdownEffect.Stop();
                break;
        }
    }

    [System.Obsolete]
    public void ChangeDashEffectColor(Color targetColor)
    {
        _dashEffect.startColor = targetColor;
    }

    public bool isPlaying(string clipName)
    {
        switch (clipName)
        {
            case Definition.VFX_DUST:
                if (_dashEffect.isPlaying)
                    return true;
                else
                    return false;
            case Definition.VFX_TAKE_ITEM:
                if (_takeItemEffect.isPlaying)
                    return true;
                else
                    return false;
            case Definition.VFX_RECOVER:
                if (_recoverEffect.isPlaying)
                    return true;
                else
                    return false;
            case Definition.VFX_DANDELION:
                if (_dandelionEffect.isPlaying)
                    return true;
                else
                    return false;
            case Definition.VFX_DASH:
                if (_dashEffect.isPlaying)
                    return true;
                else
                    return false;
            case Definition.VFX_KNOCKDOWN:
                if (_knockdownEffect.isPlaying)
                    return true;
                else
                    return false;
        }

        Debug.Log("name is fault");
        return false;
    }
}
