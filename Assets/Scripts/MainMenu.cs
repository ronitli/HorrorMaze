using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public AudioSource music;

    public Button play, instructions, exit;

    private void Start()
    {
        play.onClick.AddListener(StartGame);
        instructions.onClick.AddListener(ShowInstructions);
        exit.onClick.AddListener(QuitGame);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
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

    private void OnDestroy() {
        play.onClick.RemoveAllListeners();
        instructions.onClick.RemoveAllListeners();
        exit.onClick.RemoveAllListeners();
    }
}