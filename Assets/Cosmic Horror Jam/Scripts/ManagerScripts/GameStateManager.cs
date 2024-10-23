using KinematicCharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public PlayerKCC player;
    public Transform spawnSpot;
    public KeyItemSpawner keyItemSpawner;
    private PlayerInventory playerInventory;

    public int FPSCap;
    public float _timeScale;

    public float elevatorDelay = 3f; // Delay time in seconds
    public bool isPaused;

    private PedestalScript _pedestalScript;
    private bool _playEndCutscene;

    public GameObject _elevator;
    public GameObject _activatedSlab;


    void Awake()
    {
        _elevator.SetActive(false);
        _activatedSlab.SetActive(false);
        _playEndCutscene = false;
        Application.targetFrameRate = FPSCap;
        _pedestalScript = FindObjectOfType<PedestalScript>();
        keyItemSpawner = FindObjectOfType<KeyItemSpawner>();
        playerInventory = FindObjectOfType<PlayerInventory>();
    }

    // Start is called before the first frame update
    void Start()
    {
        isPaused = false;
        AudioManager.instance.PlayMusic(0);
    }



    private void Update()
    {
        if (!_playEndCutscene)
        {
            if (_pedestalScript.PedestalIsUnlocked())
            {
                Debug.Log("End");
                _playEndCutscene = true;

                // Start a coroutine to delay the elevator activation
                StartCoroutine(ActivateElevatorWithDelay());
            }
        }

        if (isPaused)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1.0f;
        }
    }

    private IEnumerator ActivateElevatorWithDelay()
    {
        _pedestalScript.gameObject.SetActive(false);
        _activatedSlab.SetActive(true);
        yield return new WaitForSeconds(elevatorDelay);  // Wait for the specified delay
        _activatedSlab.SetActive(false);
        _elevator.SetActive(true);  // Activate the elevator after the delay
    }

    public void RespawnPlayer()
    {
        player.Motor.SetPosition(spawnSpot.position);

        foreach (EKeyItem keyItemEnum in playerInventory.GetHeldKeyItems())
        {
            keyItemSpawner.ResetKeyPickup(keyItemEnum);
        }

        playerInventory.ClearInventory();
    }
}
