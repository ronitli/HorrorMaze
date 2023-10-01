using UnityEngine;
using Random = UnityEngine.Random;

public class BlinkingLights : MonoBehaviour
{
    public float maxDistance; // Maximum distance at which lights should blink.
    public float blinkInterval; // Time interval for blinking.
    private GameObject[] _enemies;
    
//    private bool isBlinking = false;

    [SerializeField] private new Light light;
    
    public Material on;
    public Material off;

    private float _timer;

    private MeshRenderer _mat;

    private void Awake()
    {
        _mat = gameObject.GetComponent<MeshRenderer>();
    }

    private void Start()
    {
        _enemies = GameObject.FindGameObjectsWithTag("Enemy");
        _timer = Random.Range(0.1f, 0.4f);
    }

    // Update is called once per frame
    private void Update()
    {
        foreach (var enemy in _enemies)
        {
            var distanceToPlayer = Vector3.Distance(transform.position, enemy.transform.position);
            
            // Check if the player is within the trigger zone and the lights are not already blinking.
            if (distanceToPlayer <= maxDistance)
            {
                if (_timer > 0)
                {
                    _timer -= Time.deltaTime;
                }
                else if (_timer <= 0)
                {
                    light.enabled = !light.enabled;
                    _mat.material = light.enabled ? on : off;
                    _timer = Random.Range(0.1f, 0.4f);
                }
            }
            else
            {
                light.enabled = true;
                _mat.material = on;
            }
        }
    }
}
