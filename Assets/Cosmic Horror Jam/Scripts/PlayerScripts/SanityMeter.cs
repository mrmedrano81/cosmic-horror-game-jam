using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SanityMeter : MonoBehaviour
{
    public FieldOfView _fov;

    public float _maxSanity;
    public float _currentSanity;
    public float _sanityGain;

    private void Awake()
    {
        
    }

    public void CheckIfLookingAtSpider()
    {
        if (_fov.visibleTargets.Count > 0)
        {
            // decrease sanity
        }
    }
}
