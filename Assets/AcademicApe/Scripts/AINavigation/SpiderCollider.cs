using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderCollider : MonoBehaviour
{
    // Start is called before the first frame update
    public GameStateManager _gamestateManager;
    void Start()
    {
        _gamestateManager = FindObjectOfType<GameStateManager>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Spider Hit Object" + other.gameObject.name);
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Spider Hit Player");
            //GameOverLogic
            //_gamestateManager.GameOver();
        }
    }
    

   
}
