using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public string sceneName;
    public float waitTime;


    private void Start()
    {
        StartCoroutine(LoadToMenu());
    }

    IEnumerator LoadToMenu()
    {
        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
}
