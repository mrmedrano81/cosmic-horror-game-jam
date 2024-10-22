using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{

    public List<KeyItem> keyItems = new List<KeyItem>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<EKeyItem> GetHeldKeyItems()
    {
        List<EKeyItem> keyItemList = new List<EKeyItem>();

        foreach (KeyItem keyItem in keyItems)
        {
            keyItemList.Add(keyItem.keyEnum);
        }

        return keyItemList;
    }

    public void ClearInventory()
    {
        keyItems.Clear();
    }

    public void RemoveKey(EKeyItem keyItemEnum)
    {
        KeyItem tempKeyItem = null;

        foreach (KeyItem keyItem in keyItems)
        {
            if (keyItem.keyEnum == keyItemEnum)
            {
                tempKeyItem = keyItem;
            }
        }

        if (tempKeyItem)
        {
            keyItems.Remove(tempKeyItem);
        }
    }
}
