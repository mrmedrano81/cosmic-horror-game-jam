using KinematicCharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SanityRecoveryZone : MonoBehaviour
{
    public float _sanityRecoveryInterval;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerKCC player = other.gameObject.GetComponent<PlayerKCC>();


        }
    }

    IEnumerator RecoverSanity()
    {
        yield return new WaitForSeconds(_sanityRecoveryInterval);
    }
}
