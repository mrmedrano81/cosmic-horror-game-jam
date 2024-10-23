using KinematicCharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameStateManager : MonoBehaviour
{
    public PlayerKCC playerKCC;
    public Transform spawnSpot;
    public KeyItemSpawner keyItemSpawner;
    private PlayerInventory playerInventory;
    private SanityMeter sanityMeter;

    public int FPSCap;
    public float _timeScale;

    public float elevatorDelay = 3f; // Delay time in seconds
    public bool isPaused;

    private PedestalScript _pedestalScript;
    private bool _playEndCutscene;

    public GameObject _elevator;
    public GameObject _activatedSlab;

    [Header("Game Over UI")]
    public GameObject gameOverPanel;
    public float restartDelay = 2f;


    void Awake()
    {
        _elevator.SetActive(false);
        _activatedSlab.SetActive(false);
        _playEndCutscene = false;
        Application.targetFrameRate = FPSCap;

        playerKCC = FindAnyObjectByType<PlayerKCC>();
        _pedestalScript = FindObjectOfType<PedestalScript>();
        gameOverPanel.SetActive(false);
        keyItemSpawner = FindObjectOfType<KeyItemSpawner>();
        playerInventory = FindObjectOfType<PlayerInventory>();
        sanityMeter = FindObjectOfType<SanityMeter>();
        
    }

    // Start is called before the first frame update
    void Start()
    {
        isPaused = false;
        AudioManager.instance.PlayMusic(0, 0.2f);
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

        if (sanityMeter._respawnFromInsanity)
        {
            RespawnFromInsanity();
            sanityMeter._respawnFromInsanity = false;
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

    public void RespawnFromInsanity()
    {
        playerKCC.Motor.SetPosition(spawnSpot.position);

        foreach (EKeyItem keyItemEnum in playerInventory.GetHeldKeyItems())
        {
            keyItemSpawner.ResetKeyPickup(keyItemEnum);
        }

        sanityMeter._currentSanity = sanityMeter._maxSanity;

        playerInventory.ClearInventory();
    }

    public void GameOver()
    {
        isPaused = true;
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
        ReloadGame();
    }

    public void ReloadGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
