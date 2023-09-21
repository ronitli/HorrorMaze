using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public AudioSource music;

    private void Start()
    {
        music.Play();
        DontDestroyOnLoad(music);
    }

    public void StartGame()
    {
        Debug.Log("StartGame function called");
        SceneManager.LoadScene("Gameplay");//gameplay
    }
    
    public void ShowInstructions()
    {
        Debug.Log("ShowInstructions function called");
        SceneManager.LoadScene("Instructions");//instructions
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game");
        Application.Quit();
    }
}