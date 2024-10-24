using UnityEngine;
using UnityEngine.Rendering;

public class SanityMeter : MonoBehaviour
{
    public FieldOfView _fov;
    public PlayerAudioScript _audioScript;

    [Header("Sanity Parameters")]
    public float _maxSanity = 100f; // Sanity Cap
    public float _sanityDecreaseRate = 2f; // Sanity tick damage 
    public float _currentSanity; // Current Sanity 
    public float _sanityGain; // For Brazier Sanity Regen 

    private float _ticktimer;
    [SerializeField] private float _sanityDecreaseTick = 1f;
    private bool _IsEnemySeen = false;
    private bool _playSanitySound;

    // Variables for checking zero sanity duration
    private float _zeroSanityTimer = 0f;
    [SerializeField] private float _requiredZeroSanityDuration = 5f;
    private bool _hasReachedZeroSanityDuration = false;

    [HideInInspector] public bool _respawnFromInsanity;
    [HideInInspector] public bool _triggerBlackoutFromSanity;

    private void Awake()
    {
        _triggerBlackoutFromSanity = false;
        _respawnFromInsanity = false;
        _audioScript = FindObjectOfType<PlayerAudioScript>();
        _playSanitySound = false;
        _currentSanity = _maxSanity;
        _fov = GetComponent<FieldOfView>();
        _IsEnemySeen = false;
        _ticktimer = 0f;
    }

    private void Update()
    {
        _IsEnemySeen = _EnemySeen();

        if (_IsEnemySeen)
        {
            _ticktimer += Time.deltaTime;

            if (_ticktimer > _sanityDecreaseTick)
            {
                _DecreaseSanity();
                _ticktimer = 0f;
            }
        }
        else
        {
            _IsEnemySeen = false;
        }

        if (_currentSanity < _maxSanity)
        {
            if (!_audioScript.sanitySource.isPlaying)
            {
                AudioManager.instance.PlaySFX(_audioScript.sanitySource, EPlayerSFX.Insanity, 0);
            }
            else
            {
                AdjustSanitySoundVolume();
            }
        }
        else
        {
            _audioScript.sanitySource.Stop();
        }

        // Check for zero sanity duration
        CheckZeroSanityDuration();
    }

    public void AdjustSanitySoundVolume()
    {
        float minVolume = 0.05f; // Volume at full sanity
        float maxVolume = 0.8f; // Volume at zero sanity

        float sanityPercentage = _currentSanity / _maxSanity; // Normalize sanity to a range of 0 to 1
        float volume = Mathf.Lerp(maxVolume, minVolume, sanityPercentage); // Inverse lerp (lower sanity = higher volume)

        _audioScript.sanitySource.volume = volume;
    }

    private bool _EnemySeen()
    {
        if (_fov.visibleTargets.Count > 0) // If FOV Raycast hits enemy targets and registers enemies
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void _DecreaseSanity()
    {
        _currentSanity -= _sanityDecreaseRate; // Apply Sanity Damage
        _currentSanity = Mathf.Max(0, _currentSanity); // Clamp to 0
    }

    // This method checks if the player's sanity has been 0 for the required duration
    private void CheckZeroSanityDuration()
    {
        if (_currentSanity <= 0)
        {
            // If sanity is 0, start or continue the timer
            _zeroSanityTimer += Time.deltaTime;

            if (_zeroSanityTimer >= _requiredZeroSanityDuration && !_hasReachedZeroSanityDuration)
            {
                // Player has been at 0 sanity for the required duration
                _hasReachedZeroSanityDuration = true;
                OnZeroSanityDurationReached();
            }
        }
        else
        {
            // Reset the timer if sanity goes above 0
            _zeroSanityTimer = 0f;
            _hasReachedZeroSanityDuration = false;
        }
    }

    // This method gets called when sanity has been 0 for the required duration
    private void OnZeroSanityDurationReached()
    {
        //Debug.Log("Sanity has been 0 for the required duration!");
        // Trigger insanity or other effects here

        _triggerBlackoutFromSanity = true;
    }

    public void CheckIfLookingAtSpider()
    {
        if (_fov.visibleTargets.Count > 0)
        {
            // Decrease sanity
        }
    }
}
