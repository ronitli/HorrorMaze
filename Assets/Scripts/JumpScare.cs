using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JumpScare : MonoBehaviour
{
    public Animator attackAnimator;
    private GameObject _player;
    public float jumpscareTime;
    public string sceneName;

    public GameObject camara;

    public AudioSource scream;
    private bool _screamed = false;
    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _player.SetActive(false); 
            camara.SetActive(true);
            attackAnimator.SetTrigger("jumpscare");
            if(!_screamed)
                scream.Play();
            StartCoroutine(Jumpscare());
        }
    }

    IEnumerator Jumpscare()
    {
        yield return new WaitForSeconds(jumpscareTime);
        SceneManager.LoadScene(sceneName);
    }
}
