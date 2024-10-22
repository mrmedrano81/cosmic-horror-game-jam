using KinematicCharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class PlayerInteraction : MonoBehaviour
{
    [Header("Interaction Settings")]
    public LayerMask interactableLayer;

    private SphereCollider interactionCollider;
    private float interactRange;

    [HideInInspector] public bool _interactInput;

    [Header("[DEBUG]")]
    public PlayerInventory inventory;
    public FloatingText activeFloatingText;
    public PlayerAudioScript playerAudio;

    private void Awake()
    {
        playerAudio = GetComponentInParent<PlayerAudioScript>();
        interactionCollider = GetComponent<SphereCollider>();
    }

    private void Start()
    {
        interactRange = interactionCollider.radius;
    }


    private void OnTriggerStay(Collider other)
    {
        // Perform a raycast from the player's position in the direction they are facing
        RaycastHit hit;
        Vector3 rayOrigin = Camera.main.transform.position;
        Vector3 rayDirection = Camera.main.transform.forward;

        // Raycast checks for the closest interactable object within range
        if (Physics.Raycast(rayOrigin, rayDirection, out hit, interactRange, interactableLayer))
        {
            // Ensure that the object in the trigger is also the one hit by the raycast
            if (hit.collider == other)
            {
                activeFloatingText = other.GetComponent<FloatingText>();

                if (activeFloatingText)
                {    
                    if (other.gameObject.CompareTag("KeySlot"))
                    {
                        if (PlayerHasKeyItem(other.gameObject.GetComponent<KeyPlacement>()))
                        {
                            activeFloatingText.ShowText(Camera.main.transform.position);
                        }
                    }
                    else
                    {
                        activeFloatingText.ShowText(Camera.main.transform.position);
                    }
                }

                if (_interactInput)
                {
                    Interact interactScript = other.GetComponent<Interact>();

                    if (interactScript)
                    {
                        if (other.gameObject.CompareTag("Key"))
                        {
                            AudioManager.instance.PlaySFX(playerAudio.interactionSource, EOtherSFX.Interact);
                            interactScript.CallInteract(this);
                        }
                        else if (other.gameObject.CompareTag("KeySlot"))
                        {
                            KeyPlacement targetKeySlot = other.gameObject.GetComponent<KeyPlacement>();

                            if (PlayerHasKeyItem(targetKeySlot))
                            {
                                interactScript.CallInteract(this);
                                AudioManager.instance.PlaySFX(playerAudio.interactionSource, EOtherSFX.Interact);
                                inventory.RemoveKey(targetKeySlot.keyEnum);
                            }
                        }
                        else if (other.gameObject.CompareTag("Brazier"))
                        {
                            AudioManager.instance.PlaySFX(playerAudio.interactionSource, ELightingSFX.TorchLight);
                            interactScript.CallInteract(this);
                        }
                        else
                        {
                            interactScript.CallInteract(this);
                        }
                    }
                    else
                    {
                        //Debug.Log("No interaction script for " + other.gameObject.name);
                    }
                }
            }
        }
        else
        {
            // If the raycast didn't hit, check if the player is inside the collider's bounds
            if (other.bounds.Contains(Camera.main.transform.position))
            {
                activeFloatingText = other.GetComponent<FloatingText>();

                if (activeFloatingText)
                {
                    if (other.gameObject.CompareTag("KeySlot"))
                    {
                        if (PlayerHasKeyItem(other.gameObject.GetComponent<KeyPlacement>()))
                        {
                            activeFloatingText.ShowText(Camera.main.transform.position);
                        }
                    }
                    else
                    {
                        activeFloatingText.ShowText(Camera.main.transform.position);
                    }
                }

                if (_interactInput)
                {
                    Interact interactScript = other.GetComponent<Interact>();

                    if (interactScript)
                    {
                        if (other.gameObject.CompareTag("Key"))
                        {
                            AudioManager.instance.PlaySFX(playerAudio.interactionSource, EOtherSFX.Interact);
                            interactScript.CallInteract(this);
                        }
                        else if (other.gameObject.CompareTag("KeySlot"))
                        {
                            KeyPlacement targetKeySlot = other.gameObject.GetComponent<KeyPlacement>();

                            if (PlayerHasKeyItem(targetKeySlot))
                            {
                                interactScript.CallInteract(this);
                                AudioManager.instance.PlaySFX(playerAudio.interactionSource, EOtherSFX.Interact);
                                inventory.RemoveKey(targetKeySlot.keyEnum);
                            }
                        }
                        else if (other.gameObject.CompareTag("Brazier"))
                        {
                            AudioManager.instance.PlaySFX(playerAudio.interactionSource, ELightingSFX.TorchLight);
                            interactScript.CallInteract(this);
                        }
                        else
                        {
                            interactScript.CallInteract(this);
                        }
                    }
                    else
                    {
                        //Debug.Log("No interaction script for " + other.gameObject.name);
                    }
                }
            }
            else
            {
                activeFloatingText?.HideText();
            }
        }
    }


    private void OnTriggerExit(Collider other)
    {
        //activeFloatingText?.HideText();
    }

    private void OnDrawGizmos()
    {
        // Draw the ray in the editor to visualize the interaction range
        Gizmos.color = Color.green;
        Gizmos.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * interactRange);
    }

    public void AddKeyItemToInventory(KeyItem keyItem)
    {
        inventory.keyItems.Add(keyItem);
    }

    public bool PlayerHasKeyItem(KeyPlacement keyPlacement)
    {
        List<EKeyItem> currentPlayerKeys = inventory.GetHeldKeyItems();

        foreach (EKeyItem keyEnum in currentPlayerKeys)
        {
            if (keyEnum == keyPlacement.keyEnum)
            {
                return true;
            }
        }

        return false;
    }
}