using KinematicCharacterController;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    [SerializeField] public GameObject _interactionText;
    public bool _textFacePlayerOnlyOnTrigger;
    private bool _textAppeared;

    // Start is called before the first frame update
    void Start()
    {
        _textAppeared = false;
        _interactionText.SetActive(false);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _interactionText.SetActive(true);

            Vector3 playerPos = other.gameObject.transform.position;

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
