using System;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    private Light light;

    public AudioSource sound;

    private void Awake()
    {
        light = GetComponent<Light>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            light.enabled = !light.enabled;
            sound.Play();
        }
    }
}
