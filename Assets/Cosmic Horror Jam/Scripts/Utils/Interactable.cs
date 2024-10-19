using KinematicCharacterController;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public bool _textFacePlayerOnlyOnTrigger;
    public bool _singleInteraction;
    private bool _textAppeared;

    [HideInInspector] public bool _interacted;

    [SerializeField] private GameObject _interactionText;

    // Start is called before the first frame update
    void Start()
    {
        _textAppeared = false;
        _interacted = false;
        _interactionText.SetActive(false);
    }

    private void OnTriggerStay(Collider other)
    {
        if (_interacted && _singleInteraction)
        {
            return;
        }

        if (other.gameObject.CompareTag("Player"))
        {
            _interactionText.SetActive(true);

            PlayerKCC playerController = other.gameObject.GetComponent<PlayerKCC>();

            if (_textFacePlayerOnlyOnTrigger)
            {
                if (!_textAppeared)
                {
                    Vector3 directionToPlayer = Vector3.ProjectOnPlane(-(playerController.gameObject.transform.position - _interactionText.transform.position), Vector3.up);

                    _interactionText.transform.rotation = Quaternion.LookRotation(directionToPlayer, Vector3.up);

                    _textAppeared = true;
                }
            }
            else 
            {
                Vector3 directionToPlayer = Vector3.ProjectOnPlane(-(playerController.gameObject.transform.position - _interactionText.transform.position), Vector3.up);

                _interactionText.transform.rotation = Quaternion.LookRotation(directionToPlayer, Vector3.up);
            }

            if (playerController._interact)
            {
                _interactionText.SetActive(false);
                _interacted = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _interactionText.SetActive(false);
            _textAppeared = false;
        }
    }
}
