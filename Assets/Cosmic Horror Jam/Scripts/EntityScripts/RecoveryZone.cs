using KinematicCharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoveryZone : MonoBehaviour
{
    [Header("Sanity Recovery Parameters")]
    public float _sanityRecoveryRate = 5f; //Sanity Points Restored
    public float _sanityRecoveryTickSpeed = 1f; // How long it takes to receive a tick of recovery
    private float _sanityRecoveryTimer;
    public float _sanityRecoveryInterval = 1f; //Mike's Code

    [HideInInspector] public SanityMeter SAN; // Reference SanityMeter script
    [HideInInspector] public TorchStatusScript torch; // Reference SanityMeter script

    //public float _sanityRecoveryInterval; Mike's Code

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerKCC player = other.gameObject.GetComponent<PlayerKCC>();
            SAN = other.gameObject.GetComponentInChildren<SanityMeter>();
            //Debug.Log("Player in Safe Zone");

            if (SAN != null)
            {
                //Debug.Log("Recovering SAN");
                _sanityRecoveryTimer += Time.deltaTime;
                if (_sanityRecoveryTimer > _sanityRecoveryTickSpeed)
                {
                    _sanityRecoveryTimer = 0f;
                    StartCoroutine(_RecoverSAN());
                }
            }
            else if (SAN != null) Debug.Log("Sanity Meter not referenced");
        }

        if (other.gameObject.CompareTag("Torch"))
        {
            torch = other.gameObject.GetComponent<TorchStatusScript>();

            if (torch)
            {
                torch.currentValue = torch.maxValue;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //Clear References
        if (other.gameObject.CompareTag("Player"))
        {
            //player = null;
            SAN = null;
            //Left Recovery Zone
            //Debug.Log("player left safe zone");
        }
    }

    /*   
    private void Update()
    {
        if(SanityMeter != null)
        {
            _sanityRecoveryTimer += Time.deltaTime;
            if(_sanityRecoveryTimer > _sanityRecoveryTickSpeed)
            {
                _sanityRecoveryTimer = 0f;
                StartCoroutine(_RecoverSAN());
            }
        }
    }
    */

    private IEnumerator _RecoverSAN()
    {
        SAN._currentSanity += _sanityRecoveryRate;
        SAN._currentSanity = Mathf.Min(SAN._currentSanity, 100);

        Debug.Log($"Current Sanity: {SAN._currentSanity}");

        yield return new WaitForSeconds(_sanityRecoveryInterval);
    }

    private IEnumerator _RecoverTorch()
    {
        SAN._currentSanity += _sanityRecoveryRate;
        SAN._currentSanity = Mathf.Min(SAN._currentSanity, 100);

        Debug.Log($"Current Sanity: {SAN._currentSanity}");

        yield return new WaitForSeconds(_sanityRecoveryInterval);
    }


    /*
    IEnumerator RecoverSanity()
    {
        yield return new WaitForSeconds(_sanityRecoveryInterval);
    }
    */
}
