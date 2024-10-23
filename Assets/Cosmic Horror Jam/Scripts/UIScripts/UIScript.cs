using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UIScript : MonoBehaviour
{
    public GameObject pauseMenu;

    private GameStateManager gameState;


    private void Awake()
    {
        gameState = FindObjectOfType<GameStateManager>();
    }

    private void Update()
    {
        pauseMenu.SetActive(gameState.isPaused);
    }
}
