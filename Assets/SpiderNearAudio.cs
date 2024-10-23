using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpiderNearAudio : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioSource spidernearSource;
    public AiManager ai;

    public bool _playFootsteps;

    public float walkInterval;
    public float runInterval;
    private float _currentFootstepTime;

    //AudioManager.instance.PlaySFX(spidernearSource, EPlayerSFX.Walk, 1, true);
    void Awake()
    {
        _playFootsteps = false;
        ai = GetComponent<AiManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
      if (other.gameObject.CompareTag("Player"))
        {
            if(ai.Agent.speed == ai.patrolSpeed)
            {
                AudioManager.instance.PlaySFX(spidernearSource, EPlayerSFX.Walk, 1, true);
            }
        }
    }


}
