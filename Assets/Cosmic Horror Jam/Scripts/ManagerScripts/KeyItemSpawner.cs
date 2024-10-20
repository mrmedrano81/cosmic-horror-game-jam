using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyItemSpawner : MonoBehaviour
{
    public Transform[] _spawnLocations;
    public GameObject _keyItem;
    private List<GameObject> _keyItemsArray = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        if (_spawnLocations.Length == 0 || _keyItem == null)
        {
            Debug.LogError("check spawn locations or key item prefab in KeyItemSpawner");
            //Debug.Break();
        }

        else
        {
            foreach (Transform location in _spawnLocations)
            {
                GameObject keyItem = Instantiate(_keyItem, location.position, Quaternion.identity);
                _keyItemsArray.Add(keyItem);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
