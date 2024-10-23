using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EEnding
{
    Default,
    Secret
}

public class PedestalScript : MonoBehaviour
{
    public KeyPlacement keyPlacement_1;
    public KeyPlacement keyPlacement_2;
    public KeyPlacement keyPlacement_3;

    private Dictionary<EKeyItem, KeyPlacement> keyDict = new Dictionary<EKeyItem, KeyPlacement>();

    private int _currentPlacementOrder;

    void Start()
    {
        _currentPlacementOrder = 0;

        keyDict.Add(EKeyItem.Top, keyPlacement_1);
        keyDict.Add(EKeyItem.Center, keyPlacement_2);
        keyDict.Add(EKeyItem.BottomRight, keyPlacement_3);

        keyDict[EKeyItem.Top]._isPlaced = false;

        keyDict[EKeyItem.Center]._isPlaced = false;

        keyDict[EKeyItem.BottomRight]._isPlaced = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public EEnding GetEndingEnum()
    {
        if (keyDict[EKeyItem.Top]._placementOrder == 2 &&
            keyDict[EKeyItem.Center]._placementOrder == 1 &&
            keyDict[EKeyItem.BottomRight]._placementOrder == 0)
        {
            return EEnding.Secret;
        }
        else
        {
            return EEnding.Default;
        }
    }

    public bool PedestalIsUnlocked()
    {
        foreach (EKeyItem key in keyDict.Keys)
        {
            if (!keyDict[key]._isPlaced)
            {
                return false;
            }
        }

        return true;
    }
}
