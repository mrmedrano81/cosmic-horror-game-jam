using KinematicCharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SanityRecover : MonoBehaviour
{
    //Declare Variables
    [Header("Sanity Recovery Parameters")]
    public float _sanityRecoveryRate = 5f; //Sanity Points Restored
    public float _sanityRecoveryTickSpeed = 1f; // How long it takes to receive a tick of recovery
    private float _sanityRecoveryTimer;


    [HideInInspector] public SanityMeter SanityMeter; // Reference SanityMeter script
    [HideInInspector] public PlayerKCC player; //Reference Player Script
    void Start()
    {
        //SanityMeter = player.gameObject.GetComponent<PlayerKCC>();
        
    }

    // Update is called once per frame
    void Update()
    {
        //When SAN Recovery Zone Activated
        _sanityRecoveryTimer += Time.deltaTime;
        if(_sanityRecoveryTimer > _sanityRecoveryTickSpeed)
        {
            _sanityRecoveryTimer = 0;
            _RecoverSAN(); //Call SAN Regen
        }
        
    }

    private void _RecoverSAN()
    {
        SanityMeter._currentSanity += _sanityRecoveryRate;
        SanityMeter._currentSanity = Mathf.Min(SanityMeter._currentSanity, 100);

        Debug.Log($"Current Sanit: {SanityMeter._currentSanity}");

    }
}
