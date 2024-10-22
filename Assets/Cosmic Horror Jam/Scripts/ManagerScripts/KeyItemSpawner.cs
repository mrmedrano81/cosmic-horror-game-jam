using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyItemSpawner : MonoBehaviour
{
    public GameObject keyItemPickup_1;
    public GameObject keyItemPickup_2;
    public GameObject keyItemPickup_3;

    private Dictionary<EKeyItem, GameObject> keyItemsDict = new Dictionary<EKeyItem, GameObject>();

    private void Awake()
    {
        keyItemsDict.Add(EKeyItem.BottomRight, keyItemPickup_1);
        keyItemsDict.Add(EKeyItem.Top, keyItemPickup_2);
        keyItemsDict.Add(EKeyItem.Center, keyItemPickup_3);
    }

    public void ResetKeyPickup(EKeyItem keyItem)
    {
        keyItemsDict[keyItem].SetActive(true);
    }


}
