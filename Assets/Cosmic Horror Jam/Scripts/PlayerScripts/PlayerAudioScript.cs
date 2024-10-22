using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioScript : MonoBehaviour
{
    public AudioSource footstepSource;
    public AudioSource sanitySource;
    public AudioSource interactionSource;

    public bool _playFootsteps;

    public float walkInterval;
    public float runInterval;
    private float _currentFootstepTime;

    private void Awake()
    {
        _playFootsteps = false;
    }

    public void PlayFootstepSounds(bool isRunning)
    {
        if (isRunning)
        {
            if (Time.time - _currentFootstepTime > runInterval)
            {
                AudioManager.instance.PlaySFX(footstepSource, EPlayerSFX.Walk, 1, true);
                _currentFootstepTime = Time.time;
            }
        }
        else
        {
            if (Time.time - _currentFootstepTime > walkInterval)
            {
                AudioManager.instance.PlaySFX(footstepSource, EPlayerSFX.Walk, 1, true);
                _currentFootstepTime = Time.time;
            }
        }
    }
}
