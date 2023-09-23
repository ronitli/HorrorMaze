using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public string SceneName;
    public float waitTime;


    private void Start()
    {
        StartCoroutine(loadToMenu());
    }

    IEnumerator loadToMenu()
    {
        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene(SceneName);
    }
}
