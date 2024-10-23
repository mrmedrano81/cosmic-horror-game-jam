using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{

    public List<KeyItem> keyItems = new List<KeyItem>();
    private IconScript _iconScript;

    // Start is called before the first frame update
    void Start()
    {
        _iconScript = FindObjectOfType<IconScript>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateInventoryUI();
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

            if (tempKeyItem.keyEnum == EKeyItem.BottomRight)
            {
                _iconScript.keyIcon_1.SetActive(false);
            }

            else if (tempKeyItem.keyEnum == EKeyItem.Top)
            {
                _iconScript.keyIcon_2.SetActive(false);
            }

            else if (tempKeyItem.keyEnum == EKeyItem.Center)
            {
                _iconScript.keyIcon_3.SetActive(false);
            }
        }
    }

    public void UpdateInventoryUI()
    {
        if (keyItems.Count > 0)
        {
            foreach (KeyItem keyItem in keyItems)
            {
                if (keyItem.keyEnum == EKeyItem.BottomRight)
                {
                    _iconScript.keyIcon_1.SetActive(true);
                }

                if (keyItem.keyEnum == EKeyItem.Top)
                {
                    _iconScript.keyIcon_2.SetActive(true);
                }

                if (keyItem.keyEnum == EKeyItem.Center)
                {
                    _iconScript.keyIcon_3.SetActive(true);
                }
            }
        }
        else
        {
            _iconScript.keyIcon_1.SetActive(false);
            _iconScript.keyIcon_2.SetActive(false);
            _iconScript.keyIcon_3.SetActive(false);
        }
        
    }
}
