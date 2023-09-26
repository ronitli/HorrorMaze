using System;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    private Light lightSource;

    public AudioSource sound;

    private void Awake()
    {
        lightSource = GetComponent<Light>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            lightSource.enabled = !lightSource.enabled;
            sound.Play();
        }
    }
}
