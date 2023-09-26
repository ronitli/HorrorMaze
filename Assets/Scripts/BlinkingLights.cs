using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class BlinkingLights : MonoBehaviour
{
    public float maxDistance = 10.0f; // Maximum distance at which lights should blink.
    public float blinkInterval = 1f; // Time interval for blinking.

    private GameObject[] enemies;
    
//    private bool isBlinking = false;

    [SerializeField] private Light Light;
    
    public Material On;
    public Material Off;

    private float timer;

    private MeshRenderer mat;

    private void Awake()
    {
        mat = gameObject.GetComponent<MeshRenderer>();
    }

    private void Start()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        timer = Random.Range(0.1f, 0.4f);
    }

    // Update is called once per frame
    private void Update()
    {
        foreach (var enemy in enemies)
        {
            var distanceToPlayer = Vector3.Distance(transform.position, enemy.transform.position);
            
            // Check if the player is within the trigger zone and the lights are not already blinking.
            if (distanceToPlayer <= maxDistance)
            {
                if (timer > 0)
                {
                    timer -= Time.deltaTime;
                }
                else if (timer <= 0)
                {
                    Light.enabled = !Light.enabled;
                    mat.material = Light.enabled ? On : Off;
                    timer = Random.Range(0.1f, 0.4f);
                }
            }
            else
            {
                Light.enabled = true;
                mat.material = On;
            }
        }
    }
}
