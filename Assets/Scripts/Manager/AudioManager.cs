using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("BGM")]
    [SerializeField] AudioSource BGM_Channel;
    [SerializeField] AudioClip[] BGM;

    [Header("SFX")]
    [SerializeField] AudioSource[] SFX_Channels;
    [SerializeField] AudioClip _jumpClip;
    [SerializeField] AudioClip _slidingClip;
    [SerializeField] AudioClip _takeItemClip;
    [SerializeField] AudioClip _thronClip;
    [SerializeField] AudioClip _recoverClip;
    [SerializeField] AudioClip _downhillClip;
    [SerializeField] AudioClip _dandelionClip;
    [SerializeField] AudioClip _dashClip;
    [SerializeField] AudioClip _dashUpgradeClip;
    [SerializeField] AudioClip _popUpClip;
    [SerializeField] AudioClip _selectClip;

    [Header("Special Channel")]
    [SerializeField] AudioSource walkChannel;
    [SerializeField] AudioClip _walkClip;

    [SerializeField] float _walkPitchValue = 1.75f;

    const int _endingIndex = 6;

    public void PlayBGM(int stageIndex)
    {
        BGM_Channel.clip = BGM[stageIndex];
        BGM_Channel.loop = true;

        switch (stageIndex)
        {
            case 0:
                BGM_Channel.volume = 0.15f;
                break;
            case 1:
                BGM_Channel.volume = 0.2f;
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
            case 5:
                break;
            case 6:
                BGM_Channel.volume = 1f;
                break;
        }

        BGM_Channel.Play();
    }

    public void PlayEndingBGM()
    {
        PlayBGM(_endingIndex);
    }

    public void PlaySFX(string clipName)
    {
        AudioSource tempChannel = null;

        switch (clipName)
        {
            case Definition.JUMP_CLIP:
                tempChannel = selectChannel();
                tempChannel.clip = _jumpClip;
                tempChannel.volume = Definition.JUMP_VOLUME;
                tempChannel.Play();
                break;
            case Definition.SLIDING_CLIP:
                tempChannel = selectChannel();
                tempChannel.clip = _slidingClip;
                tempChannel.volume = Definition.SLIDING_VOLUME;
                tempChannel.Play();
                break;
            case Definition.TAKE_ITEM_CLIP:
                tempChannel = selectChannel();
                tempChannel.clip = _takeItemClip;
                tempChannel.volume = Definition.ITEM_VOLUME;
                tempChannel.Play();
                break;
            case Definition.THORN_CLIP:
                tempChannel = selectChannel();
                tempChannel.clip = _thronClip;
                tempChannel.volume = Definition.THORN_VOLUME;
                tempChannel.Play();
                break;
            case Definition.RECOVER_CLIP:
                tempChannel = selectChannel();
                tempChannel.clip = _recoverClip;
                tempChannel.volume = Definition.RECOVER_VOLUME;
                tempChannel.Play();
                break;
            case Definition.DOWNHILL_CLIP:
                tempChannel = selectChannel();
                tempChannel.clip = _downhillClip;
                tempChannel.volume = Definition.DOWNHILL_VOLUME;
                tempChannel.Play();
                break;
            case Definition.DANDELION_CLIP:
                tempChannel = selectChannel();
                tempChannel.clip = _dandelionClip;
                tempChannel.volume = Definition.DANDELION_VOLUME;
                tempChannel.Play();
                break;
            case Definition.DASH_CLIP:
                tempChannel = selectChannel();
                tempChannel.clip = _dashClip;
                tempChannel.volume = Definition.DASH_VOLUME;
                tempChannel.Play();
                break;
            case Definition.DASH_UPGRADE_CLIP:
                tempChannel = selectChannel();
                tempChannel.clip = _dashUpgradeClip;
                tempChannel.volume = Definition.DASH_LEVEL_UP_VOLUME;
                tempChannel.Play();
                break;
            case Definition.POP_UP_CLIP:
                tempChannel = selectChannel();
                tempChannel.clip = _popUpClip;
                tempChannel.volume = Definition.POP_UP_VOLUME;
                tempChannel.Play();
                break;
            case Definition.SELECT_CLIP:
                tempChannel = selectChannel();
                tempChannel.clip = _selectClip;
                tempChannel.volume = Definition.SELECT_VOLUME;
                tempChannel.Play();
                break;
        }
    }

    public void PauseAllSFXChannel()
    {
        for (int i = 0; i < SFX_Channels.Length; i++)
        {
            SFX_Channels[i].Pause();
        }

        PauseWalkCahnnel();
    }

    public void PlayAllSFXChannel()
    {
        for (int i = 0; i < SFX_Channels.Length; i++)
        {
            SFX_Channels[i].Play();
        }
    }

    AudioSource selectChannel()
    {
        for (int i = 0; i < SFX_Channels.Length; i++)
        {
            if (!SFX_Channels[i].isPlaying)
            {
                return SFX_Channels[i];
            }
        }

        Debug.Log("can't allocate audio channel");
        return null;
    }

    public void PlayWalkChannel()
    {
        if (!walkChannel.isPlaying && Time.timeScale != 0)
        {
            walkChannel.Play();
        }
    }

    public void PauseWalkCahnnel()
    {
        walkChannel.Pause();
    }

    public void InceraseWalkChannelPitch()
    {
        walkChannel.pitch = _walkPitchValue;
    }

    public void SetDefaultPitch()
    {
        walkChannel.pitch = 1f;

    }
}
