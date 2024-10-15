using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public int FPSCap;
    public float _timeScale;

    // Start is called before the first frame update
    void Awake()
    {
        Application.targetFrameRate = FPSCap;
    }

    private void Update()
    {
        //Time.timeScale = _timeScale;
    }
}
