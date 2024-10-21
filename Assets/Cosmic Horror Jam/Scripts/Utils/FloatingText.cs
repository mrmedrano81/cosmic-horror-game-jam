using KinematicCharacterController;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    [SerializeField] public GameObject _interactionText;
    public bool _textFacePlayerOnlyOnTrigger;

    [Header("For Raycast Interaction")]
    public bool _usesRaycastInteraction;

    private bool _textAppeared;

    [HideInInspector] public bool _playerFacingObject;

    // Start is called before the first frame update
    void Start()
    {
        _playerFacingObject = false;
        _textAppeared = false;
        _interactionText.SetActive(false);
    }

    private void Update()
    {

    }

    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.gameObject.CompareTag("Player") && !_usesRaycastInteraction)
    //    {
    //        _interactionText.SetActive(true);

    //        Vector3 playerPos = other.gameObject.transform.position;

    //        if (_textFacePlayerOnlyOnTrigger)
    //        {
    //            if (!_textAppeared)
    //            {
    //                Vector3 directionToPlayer = Vector3.ProjectOnPlane(-(playerPos - _interactionText.transform.position), Vector3.up);

    //                _interactionText.transform.rotation = Quaternion.LookRotation(directionToPlayer, Vector3.up);

    //                _textAppeared = true;
    //            }
    //        }
    //        else 
    //        {
    //            Vector3 directionToPlayer = Vector3.ProjectOnPlane(-(playerPos - _interactionText.transform.position), Vector3.up);

    //            _interactionText.transform.rotation = Quaternion.LookRotation(directionToPlayer, Vector3.up);
    //        }
    //    }
    //}

    public void ShowText(Vector3 playerPos)
    {
        _interactionText.SetActive(true);

        if (_textFacePlayerOnlyOnTrigger)
        {
            if (!_textAppeared)
            {
                Vector3 directionToPlayer = Vector3.ProjectOnPlane(-(playerPos - _interactionText.transform.position), Vector3.up);

                _interactionText.transform.rotation = Quaternion.LookRotation(directionToPlayer, Vector3.up);

                _textAppeared = true;
            }
        }
        else
        {
            Vector3 directionToPlayer = Vector3.ProjectOnPlane(-(playerPos - _interactionText.transform.position), Vector3.up);

            _interactionText.transform.rotation = Quaternion.LookRotation(directionToPlayer, Vector3.up);
        }
    }


    public void HideText()
    {
        _interactionText.SetActive(false);
        _textAppeared = false;
    }

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.gameObject.CompareTag("Player") && !_usesRaycastInteraction)
    //    {
    //        _interactionText.SetActive(false);
    //        _textAppeared = false;
    //    }
    //}
}
