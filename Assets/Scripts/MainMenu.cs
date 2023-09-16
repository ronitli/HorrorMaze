using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        Debug.Log("StartGame function called");
        SceneManager.LoadScene(1);//gameplay
    }
    
    public void ShowInstructions()
    {
        Debug.Log("ShowInstructions function called");
        SceneManager.LoadScene(2);//instructions
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game");
        Application.Quit();
    }
}