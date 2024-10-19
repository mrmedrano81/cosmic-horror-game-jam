using KinematicCharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [HideInInspector] public bool _interactInput;
    public PlayerInventory inventory;

    private void OnTriggerStay(Collider other)
    {
        if (_interactInput)
        {
            Interact interactScript = other.gameObject.transform.GetComponent<Interact>();

            if (interactScript)
            {
                interactScript.CallInteract(this);
            }
            else
            {
                Debug.Log("No interaction script for " + other.gameObject.name);
            }
        }
    }

    public void AddKeyItemToInventory(KeyItem keyItem)
    {
        inventory.keyItems.Add(keyItem);
    }
}
