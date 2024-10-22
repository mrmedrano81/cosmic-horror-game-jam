using KinematicCharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameStateManager : MonoBehaviour
{
    public PlayerKCC player;
    public Transform spawnSpot;

    public int FPSCap;
    public float _timeScale;
    public bool isPaused;

    private PedestalScript _pedestalScript;
    private bool _playEndCutscene;

    public GameObject _elevator;

    [Header("Game Over UI")]
    public GameObject gameOverPanel;
    public float restartDelay = 2f;


    void Awake()
    {
        _elevator.SetActive(false);
        _playEndCutscene = false;
        Application.targetFrameRate = FPSCap;
        _pedestalScript = FindObjectOfType<PedestalScript>();
        gameOverPanel.SetActive(false);
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
                _pedestalScript.gameObject.SetActive(false);
                _elevator.SetActive(true);
            }
        }
        //Time.timeScale = _timeScale;

        if (isPaused)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1.0f;
        }
    }

    public void RespawnPlayer()
    {
        player.Motor.SetPosition(spawnSpot.position);
    }

    public void GameOver()
    {
        isPaused = true;
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ReloadGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
