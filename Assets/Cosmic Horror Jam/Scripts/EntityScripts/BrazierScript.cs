using KinematicCharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrazierScript : MonoBehaviour
{
    public GameObject _unlitKindling;
    public GameObject _litKindling;
    public GameObject _floatingText;

    public Interact _interactionScript;

    public bool _lit;
    public bool _facedPlayer;

    private void Awake()
    {
        _unlitKindling.SetActive(true);
        _litKindling.SetActive(false);

        _lit = false;
        _facedPlayer = false;
    }

    private void OnEnable()
    {
        Interact check = GetComponent<Interact>();

        if (check)
        {
            _interactionScript = check;
            _interactionScript.GetInteractEvent.HasInteracted += LightBrazier;
        }
        else
        {
            check = GetComponentInChildren<Interact>();

            if (check)
            {
                _interactionScript = check;
                _interactionScript.GetInteractEvent.HasInteracted += LightBrazier;
            }
            else
            {
                Interact addComponent = gameObject.AddComponent<Interact>();

                _interactionScript = addComponent;
                _interactionScript.GetInteractEvent.HasInteracted += LightBrazier;
            }
        }
    }

    private void OnDisable()
    {
        if (_interactionScript)
        {
            _interactionScript.GetInteractEvent.HasInteracted -= LightBrazier;
        }
    }

    public void LightBrazier()
    {
        _litKindling.SetActive(true);
        _unlitKindling.SetActive(false);
        _floatingText.SetActive(false);

        gameObject.SetActive(false);
    }
}
