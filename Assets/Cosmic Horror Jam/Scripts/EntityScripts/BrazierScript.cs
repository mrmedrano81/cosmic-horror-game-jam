using KinematicCharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrazierScript : MonoBehaviour
{
    public GameObject _brazierFire;
    public GameObject _sanityTriggerZone;
    public FloatingText _floatingText;

    [HideInInspector] public Interact _interactionScript;

    public bool _lit;
    public bool _facedPlayer;

    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _brazierFire.SetActive(false);

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
        _brazierFire.SetActive(true);
        _floatingText.enabled = false;
        _floatingText._interactionText.SetActive(false);
        _sanityTriggerZone.SetActive(true);

        AudioManager.instance.PlaySFX(_audioSource, ELightingSFX.BrazierIdle, 1);

        gameObject.SetActive(false);
    }
}
