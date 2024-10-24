using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuScript : MonoBehaviour
{
    public GameObject mainPauseMenu;
    public GameObject optionsPauseMenu;

    private void OnEnable()
    {
        mainPauseMenu.SetActive(true);
        optionsPauseMenu.SetActive(false);
    }

    private void OnDisable()
    {
        mainPauseMenu.SetActive(true);
        optionsPauseMenu.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadMenuScene()
    {
        AudioManager.instance.PlayMusic(0, 0);
        SceneManager.LoadScene(0);
    }
}
