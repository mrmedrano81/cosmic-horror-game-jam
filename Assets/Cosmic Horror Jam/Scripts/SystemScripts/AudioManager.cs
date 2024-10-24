using System;
using System.Collections.Generic;
using UnityEngine;

#region --- SFX Enums ---

public enum EPlayerSFX
{
    Walk,
    Insanity,
    Die
}

public enum EEnemySFX
{
    SpiderFootstep,
    SpiderIdle,
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

    [Header("General Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource musicNonLoopSource;
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
    [SerializeField] private AudioClip[] PlayerFootstepSFX;
    [SerializeField] private AudioClip[] PlayerInsanitySFX;
    [SerializeField] private AudioClip[] PlayerDieSFX;

    [Header("Spider SFX")]
    [SerializeField] private AudioClip[] SpiderFootstepsSFX;
    [SerializeField] private AudioClip[] SpiderIdleSFX;
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
        PlayerSFX.Add(EPlayerSFX.Walk, PlayerFootstepSFX);
        PlayerSFX.Add(EPlayerSFX.Insanity, PlayerInsanitySFX);
        PlayerSFX.Add(EPlayerSFX.Die, PlayerDieSFX);

        EnemySFX.Add(EEnemySFX.SpiderFootstep, SpiderFootstepsSFX);
        EnemySFX.Add(EEnemySFX.SpiderIdle, SpiderIdleSFX);
        EnemySFX.Add(EEnemySFX.SpiderAttack, SpiderAttackSFX);

        LightingSFX.Add(ELightingSFX.TorchIdle, TorchIdleSFX);
        LightingSFX.Add(ELightingSFX.TorchLight, TorchLightSFX);
        LightingSFX.Add(ELightingSFX.BrazierIdle, BrazierIdleSFX);
        LightingSFX.Add(ELightingSFX.BrazierLight, BrazierLightSFX);

        OtherSFX.Add(EOtherSFX.Interact, InteractSFX);
    }

    public void PlayMusic(int musicNumber, float volume = 1, bool loop = true)
    {
        if (loop)
        {
            musicSource.clip = musicList[musicNumber];
            musicSource.volume = volume;
            musicSource.Play();
        }
        else
        {
            musicNonLoopSource.clip = musicList[musicNumber];
            musicNonLoopSource.volume = volume;
            musicNonLoopSource.Play();
        }
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