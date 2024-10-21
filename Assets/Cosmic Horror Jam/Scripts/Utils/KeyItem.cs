using KinematicCharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EKeyItem
{
    Top,
    Center,
    BottomRight
}

public class KeyItem : MonoBehaviour
{
    public EKeyItem keyEnum;
    private Interact pickupFromInteract;

    private void OnEnable()
    {
        Interact check = GetComponent<Interact>();

        if (check)
        {
            pickupFromInteract = check;
            pickupFromInteract.GetInteractEvent.HasInteracted += InteractPickup;
        }
        else
        {
            check = GetComponentInChildren<Interact>();

            if (check)
            {
                pickupFromInteract = check;
                pickupFromInteract.GetInteractEvent.HasInteracted += InteractPickup;
            }
            else
            {
                Interact addComponent = gameObject.AddComponent<Interact>();

                pickupFromInteract = addComponent;
                pickupFromInteract.GetInteractEvent.HasInteracted += InteractPickup;
            }
        }
    }

    private void OnDisable()
    {
        if (pickupFromInteract)
        {
            pickupFromInteract.GetInteractEvent.HasInteracted -= InteractPickup;
        }
    }

    public void InteractPickup()
    {
        GiveKeyItem(pickupFromInteract.GetPlayer);
    }

    public void GiveKeyItem(PlayerInteraction player)
    {
        player.AddKeyItemToInventory(this);
        gameObject.SetActive(false);
    }

}
