using System;
using System.Collections.Generic;
using UnityEngine;

#region --- SFX Enums ---

public enum EPlayerSFX
{
    Walk,
    Run,
    Jump,
    Breathing,
    Insanity,
    Heartbeat,
    Die
}

public enum EEnemySFX
{
    SpiderPatrol,
    SpiderChase,
    SpiderSpot,
    SpiderAttack
}

public enum ELightingSFX
{
    TorchIdle,
    BrazierIdle,
    BrazierLight,
    TorchLight,
}

public enum EOtherSFX
{
    Interact
}

#endregion

[HelpURL("https://www.youtube.com/watch?v=QuXqyHpquLg&t=7s")]
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("References")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource UIAudioSource;

    private Dictionary<string, AudioSource> audioDict;
    private Dictionary<string, AudioClip[]> sfxDict;

    [SerializeField] private Dictionary<EPlayerSFX, AudioClip[]> PlayerSFX = new Dictionary<EPlayerSFX, AudioClip[]>();
    [SerializeField] private Dictionary<EEnemySFX, AudioClip[]> EnemySFX = new Dictionary<EEnemySFX, AudioClip[]>();
    [SerializeField] private Dictionary<ELightingSFX, AudioClip[]> LightingSFX = new Dictionary<ELightingSFX, AudioClip[]>();
    [SerializeField] private Dictionary<EOtherSFX, AudioClip[]> OtherSFX = new Dictionary<EOtherSFX, AudioClip[]>();

    [Header("Music")]
    [SerializeField] private AudioClip[] musicList;

    [Header("UI SFX")]
    [SerializeField] private AudioClip[] buttonSfxList;

    [Header("Player SFX")]
    [SerializeField] private AudioClip[] PlayerWalkSFX;
    [SerializeField] private AudioClip[] PlayerRunSFX;
    [SerializeField] private AudioClip[] PlayerJumpSFX;
    [SerializeField] private AudioClip[] PlayerBreathingSFX;
    [SerializeField] private AudioClip[] PlayerInsanitySFX;
    [SerializeField] private AudioClip[] PlayerHeartbeatSFX;
    [SerializeField] private AudioClip[] PlayerDieSFX;

    [Header("Spider SFX")]
    [SerializeField] private AudioClip[] SpiderPatrolSFX;
    [SerializeField] private AudioClip[] SpiderChaseSFX;
    [SerializeField] private AudioClip[] SpiderSpotSFX;
    [SerializeField] private AudioClip[] SpiderAttackSFX;

    [Header("Lighting SFX")]
    [SerializeField] private AudioClip[] TorchIdleSFX;
    [SerializeField] private AudioClip[] TorchLightSFX;

    [SerializeField] private AudioClip[] BrazierIdleSFX;
    [SerializeField] private AudioClip[] BrazierLightSFX;

    [Header("Other SFX")]
    [SerializeField] private AudioClip[] InteractSFX;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeSFXDicts();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeSFXDicts()
    {
        PlayerSFX.Add(EPlayerSFX.Walk, PlayerWalkSFX);
        PlayerSFX.Add(EPlayerSFX.Run, PlayerRunSFX);
        PlayerSFX.Add(EPlayerSFX.Jump, PlayerJumpSFX);
        PlayerSFX.Add(EPlayerSFX.Breathing, PlayerBreathingSFX);
        PlayerSFX.Add(EPlayerSFX.Insanity, PlayerInsanitySFX);
        PlayerSFX.Add(EPlayerSFX.Die, PlayerDieSFX);

        EnemySFX.Add(EEnemySFX.SpiderPatrol, SpiderPatrolSFX);
        EnemySFX.Add(EEnemySFX.SpiderSpot, SpiderSpotSFX);
        EnemySFX.Add(EEnemySFX.SpiderChase, SpiderChaseSFX);
        EnemySFX.Add(EEnemySFX.SpiderAttack, SpiderAttackSFX);

        LightingSFX.Add(ELightingSFX.TorchIdle, TorchIdleSFX);
        LightingSFX.Add(ELightingSFX.TorchLight, TorchLightSFX);
        LightingSFX.Add(ELightingSFX.BrazierIdle, BrazierIdleSFX);
        LightingSFX.Add(ELightingSFX.BrazierLight, BrazierLightSFX);

        OtherSFX.Add(EOtherSFX.Interact, InteractSFX);
    }

    public void PlayMusic(int musicNumber, float volume = 1)
    {
        musicSource.clip = musicList[musicNumber];
        musicSource.volume = volume;
        musicSource.Play();
    }

    public void PlayButtonSFX(int buttonNumber, float volume = 1)
    {
        UIAudioSource.clip = buttonSfxList[buttonNumber];
        UIAudioSource.volume = volume;
        UIAudioSource.Play();
    }

    public void PlaySFX(AudioSource audioSource, Enum sfxEnum, float volume = 1, bool randomSound = false)
    {
        AudioClip[] clips = null;

        switch (sfxEnum)
        {
            case EPlayerSFX playerSFXEnum:
                PlayerSFX.TryGetValue(playerSFXEnum, out clips);
                break;

            case EEnemySFX enemySFXEnum:
                EnemySFX.TryGetValue(enemySFXEnum, out clips);
                break;

            case ELightingSFX lightingSFXEnum:
                LightingSFX.TryGetValue(lightingSFXEnum, out clips);
                break;

            case EOtherSFX otherSFXEnum:
                OtherSFX.TryGetValue(otherSFXEnum, out clips);
                break;

            default:
                Debug.LogWarning("Invalid enum type passed to PlaySFX.");
                return;
        }

        if (clips == null || clips.Length == 0)
        {
            Debug.LogWarning($"No AudioClips found for key '{sfxEnum}'.");
            return;
        }

        audioSource.clip = randomSound ? clips[UnityEngine.Random.Range(0, clips.Length)] : clips[0];
        audioSource.volume = volume;
        audioSource.Play();
    }
}