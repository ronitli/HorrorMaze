using UnityEngine;

public class Flashlight : MonoBehaviour
{
    private Light _lightSource;

    public AudioSource sound;

    private void Awake()
    {
        _lightSource = GetComponent<Light>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            _lightSource.enabled = !_lightSource.enabled;
            sound.Play();
        }
    }
}
