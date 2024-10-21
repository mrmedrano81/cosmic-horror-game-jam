using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public int FPSCap;
    public float _timeScale;
    public bool isPaused;

    private PedestalScript _pedestalScript;


    void Awake()
    {
        Application.targetFrameRate = FPSCap;
        _pedestalScript = FindObjectOfType<PedestalScript>();
    }

    // Start is called before the first frame update
    void Start()
    {
        isPaused = false;
        //AudioManager.instance.PlayMusic(0);
    }

    private void Update()
    {
        if (_pedestalScript.PedestalIsUnlocked())
        {
            Debug.Log("End");
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
}
