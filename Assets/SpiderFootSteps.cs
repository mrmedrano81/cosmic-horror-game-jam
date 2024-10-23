using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SpiderFootSteps : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioSource spiderfootstepsSource;
    public AiManager ai;

    public bool _isplayingfootsteps;

    [Header ("Steps Sound Interval")]
    public float walkInterval;
    public float chaseInterval;
    public float attackInterval;
    private float _currentFootstepTime;
    private float _currentInterval;

    [Header("State Tracker")]
    public bool patrol;
    public bool chase;
    public bool attack;

    void Awake()
    {
        _isplayingfootsteps = false;

        ai = FindObjectOfType<AiManager>();
        spiderfootstepsSource = FindObjectOfType<AudioSource>();

        if (spiderfootstepsSource != null) { Debug.Log("AudioSource not rerefenced"); }

    }

    // Update is called once per frame
    void Update()
    {
        patrol = ai.IsPatrolState;
        chase = ai.IsChaseState;
        attack = ai.IsAttackState;
    }

    

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!_isplayingfootsteps)
            {
                StartCoroutine(PlayFootsteps());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StopCoroutine(PlayFootsteps());
            _isplayingfootsteps = false;
        }
    }

    private IEnumerator PlayFootsteps()
    {
        _isplayingfootsteps = true;
        while (_isplayingfootsteps)
        {
            GetFootstepInterval();
            AudioManager.instance.PlaySFX(spiderfootstepsSource, EEnemySFX.SpiderFootstep, 1, true);

            yield return new WaitForSeconds(_currentInterval);

        }
    }

    private void GetFootstepInterval()
    {

        if (ai.IsPatrolState)
        {
            Debug.Log("Returned patrol");
            _currentInterval = walkInterval;
        }
        else if (ai.IsChaseState)
        {
            Debug.Log("Returned chase");
            _currentInterval = walkInterval;
        }
        else if (ai.IsAttackState)
        {
            Debug.Log("Returned attack");
            _currentInterval = attackInterval;
        }
    }
}
