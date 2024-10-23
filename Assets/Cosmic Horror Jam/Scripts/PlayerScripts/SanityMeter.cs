using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SanityMeter : MonoBehaviour
{
    public FieldOfView _fov;

    public PlayerAudioScript _audioScript;

    [Header("Sanity Parameters")]
    public float _maxSanity = 100f; //Sanity Cap
    public float _sanityDecreaseRate = 2f; //Sanity tick damage 
    public float _currentSanity; //Current Sanity 
    public float _sanityGain; //For Brazier Sanity Regen 

    private float _ticktimer;
    [SerializeField] private float _sanityDecreaseTick = 1f;
    private bool _IsEnemySeen = false;

    private bool _playingSanitySound;

    private void Awake()
    {
        _audioScript = FindObjectOfType<PlayerAudioScript>();
        _playingSanitySound = false;
        _currentSanity = _maxSanity;
        _fov = GetComponent<FieldOfView>();
        _IsEnemySeen = false;
        _ticktimer = 0f;
    }

    private void Update()
    {
        _IsEnemySeen = _EnemySeen();

        if( _IsEnemySeen)
        {
            Debug.Log("Sanity Taking Hits");
            _ticktimer += Time.deltaTime;
            if( _ticktimer > _sanityDecreaseTick)
            {
                _DecreaseSanity();
                _ticktimer = 0f;    
            }
        }

        else _IsEnemySeen = false;
    }

    private bool _EnemySeen()
    {
        if (_fov.visibleTargets.Count > 0) // If FOV Raycast hits enemy targets and registers enemies
        {
            return true;
        }

        else { return false; }
    }

    private void _DecreaseSanity()
    {
        _currentSanity -= _sanityDecreaseRate; //Apply Sanity Damage
        _currentSanity = Mathf.Max(0, _currentSanity); //Clamp to 0

        Debug.Log($"Current Sanity: { _currentSanity}");

        if(_currentSanity <= 0)
            {
                Debug.Log("Insanity Sets In");
                //Respawn Logic
            }
    }

    public void CheckIfLookingAtSpider()
    {
        if (_fov.visibleTargets.Count > 0)
        {
            // decrease sanity
        }
    }
}
