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
    private ElevatorScript elevatorScript;
    private PlayerAudioScript playerAudio;

    public int FPSCap;
    public float _timeScale;

    public float elevatorDelay = 3f; // Delay time in seconds
    public bool isPaused;

    private PedestalScript _pedestalScript;
    private bool _startElevatorMoveSequence;

    public GameObject _elevator;
    public GameObject _activatedSlab;

    [Header("Game Over UI")]
    public GameObject gameOverPanel;
    public float restartDelay = 2f;


    void Awake()
    {
        _elevator.SetActive(false);
        _activatedSlab.SetActive(false);
        _startElevatorMoveSequence = false;
        Application.targetFrameRate = FPSCap;

        playerKCC = FindAnyObjectByType<PlayerKCC>();
        _pedestalScript = FindObjectOfType<PedestalScript>();
        gameOverPanel.SetActive(false);
        keyItemSpawner = FindObjectOfType<KeyItemSpawner>();
        playerInventory = FindObjectOfType<PlayerInventory>();
        sanityMeter = FindObjectOfType<SanityMeter>();
        elevatorScript = FindObjectOfType<ElevatorScript>();
        playerAudio = FindObjectOfType<PlayerAudioScript>();
        
    }

    // Start is called before the first frame update
    void Start()
    {
        isPaused = false;
        AudioManager.instance.PlayMusic(0, 0.2f);
    }



    private void Update()
    {
        if (elevatorScript)
        {
            if (elevatorScript._doorClosed)
            {
                
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }

        if (!_startElevatorMoveSequence)
        {
            if (_pedestalScript.PedestalIsUnlocked())
            {
                //Debug.Log("End");
                _startElevatorMoveSequence = true;

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

        elevatorScript = FindObjectOfType<ElevatorScript>();
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
        //isPaused = true;
        gameOverPanel.SetActive(true);
        //Time.timeScale = 0f;
        // Start a coroutine to reload the game after a delay

        AudioManager.instance.PlaySFX(playerAudio.interactionSource, EPlayerSFX.Die, 0.2f);
        
        StartCoroutine(ReloadGameAfterDelay());
    }

    private IEnumerator ReloadGameAfterDelay()
    {
        // Wait for the specified delay (restartDelay)
        yield return new WaitForSecondsRealtime(restartDelay);

        // After the delay, reload the game
        ReloadGame();
    }

    public void ReloadGame()
    {
        //Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
