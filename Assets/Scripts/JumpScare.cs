using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JumpScare : MonoBehaviour
{
    public Animator attackAnimator;
    private GameObject player;
    public float jumpscareTime;
    public string sceneName;

    public GameObject camara;

    public AudioSource scream;
    private bool screamed = false;
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player.SetActive(false); 
            camara.SetActive(true);
            attackAnimator.SetTrigger("jumpscare");
            if(!screamed)
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
