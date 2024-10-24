using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public GameObject _mainMenuScreen;
    public GameObject _optionsMenuScreen;


    // Start is called before the first frame update
    void Awake()
    {
        _mainMenuScreen.SetActive(true);
        _optionsMenuScreen.SetActive(false);
    }

    private void Start()
    {
        AudioManager.instance.PlayMusic(2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

}
