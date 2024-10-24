using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(FloatingText))]
[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Interact))]
[RequireComponent(typeof(Rigidbody))]

public class KeyPlacement : MonoBehaviour
{
    //public Transform placementLocation;
    public EKeyItem keyEnum;
    private MeshRenderer _keyMeshRenderer;
    private FloatingText _floatingText;
    private SphereCollider _collider;
    private Interact _interactScript;
    private Rigidbody _rb;

    [Header("DEBUG")]
    public bool _isPlaced;
    [HideInInspector] public int _placementOrder = -1;

    private void Awake()
    {
        _keyMeshRenderer = GetComponent<MeshRenderer>();
        _floatingText = GetComponent<FloatingText>();
        _collider = GetComponent<SphereCollider>();
        _rb = GetComponent<Rigidbody>();

        _collider.isTrigger = true;

        _keyMeshRenderer.enabled = false;
        _collider.enabled = true;
        _floatingText.enabled = true;
        _floatingText._interactionText.SetActive(false);
        _isPlaced = false;
        _rb.useGravity = false;
    }

    private void OnEnable()
    {
        Interact check = GetComponent<Interact>();

        if (check)
        {
            _interactScript = check;
            _interactScript.GetInteractEvent.HasInteracted += PlaceKey;
        }
        else
        {
            check = GetComponentInChildren<Interact>();

            if (check)
            {
                _interactScript = check;
                _interactScript.GetInteractEvent.HasInteracted += PlaceKey;
            }
            else
            {
                Interact addComponent = gameObject.AddComponent<Interact>();

                _interactScript = addComponent;
                _interactScript.GetInteractEvent.HasInteracted += PlaceKey;
            }
        }
    }

    private void OnDisable()
    {
        if (_interactScript)
        {
            _interactScript.GetInteractEvent.HasInteracted -= PlaceKey;
        }
    }

    public void PlaceKey()
    {
        if (!_isPlaced)
        {
            _isPlaced = true;
            _floatingText._interactionText.SetActive(false);
            _floatingText.enabled = false;
            _keyMeshRenderer.enabled = true;
            _collider.enabled = false;
        }
    }
}
