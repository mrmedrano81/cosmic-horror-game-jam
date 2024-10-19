using KinematicCharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrazierScript : MonoBehaviour
{
    public GameObject _unlitKindling;
    public GameObject _litKindling;

    public Interactable _interactable;

    public bool _lit;
    public bool _facedPlayer;

    private void Awake()
    {
        _interactable = GetComponent<Interactable>();

        _unlitKindling.SetActive(true);
        _litKindling.SetActive(false);

        _lit = false;
        _facedPlayer = false;
    }

    public void LightBrazier()
    {
        _litKindling.SetActive(true);
        _unlitKindling.SetActive(false);
    }

    private void Update()
    {
        if (_interactable._interacted && !_lit)
        {
            LightBrazier();
            _lit = true;
        }
    }
}
