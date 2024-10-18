using KinematicCharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrazierScript : MonoBehaviour
{
    public GameObject _unlitKindling;
    public GameObject _litKindling;
    public GameObject _interactionText;
    public bool _lit;
    public bool _facedPlayer;

    private void Awake()
    {
        _interactionText.SetActive(false);

        _unlitKindling.SetActive(true);
        _litKindling.SetActive(false);

        _lit = false;
        _facedPlayer = false;
    }

    public void LightBrazier()
    {
        _litKindling.SetActive(true);
        _unlitKindling.SetActive(false);

        _lit = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !_lit)
        {
            _interactionText.SetActive(true);

            PlayerKCC playerController = other.gameObject.GetComponent<PlayerKCC>();

            if (!_facedPlayer)
            {
                Vector3 directionToPlayer = Vector3.ProjectOnPlane(-(playerController.gameObject.transform.position - _interactionText.transform.position), Vector3.up);

                _interactionText.transform.rotation = Quaternion.LookRotation(directionToPlayer, Vector3.up);

                _facedPlayer= true;
            }

            if (playerController._interact)
            {
                LightBrazier();
                _interactionText.SetActive(false);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _interactionText.SetActive(false);

            _facedPlayer = false ;
        }
    }
}
