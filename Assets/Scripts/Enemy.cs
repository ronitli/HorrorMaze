using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    
    public float updateRate = 0.1f;
    public NavMeshAgent enemy;
    private GameObject _playerTarget;
    public NavMeshTriangulation Triangulation;

    public EnemyLineOfSightChecker lineOfSightChecker;

    // Control variables for random movement
//    private bool isFollowingPlayer = true; // Start by following the player
//    private float randomMoveTimer = 0f;
//    private float randomMoveDuration = 5f; // Adjust this as needed
//    private Vector3 randomMoveTarget; // Target for random movement

    // Idle movement
    public float idleLocationRadius = 4f;
    public float idleMovementMultiplier = 0.5f;

    
    public EnemyState defaultState = EnemyState.Idle;
    private EnemyState _state;

    private bool _screamed;


    [SerializeField] private float footstepDistance = 50f;
    private AudioSource _steps;
    public float maxVolume = 1f; // Maximum volume when close to the player.
    public float minVolume = 0.1f; // Minimum volume when far from the player.
    public float maxDistance = 50f; // Maximum distance at which footsteps are heard.

    public AudioSource attackScream;

    private Coroutine _followCoroutine;
    public EnemyState state
    {
        get
        {
            return _state;
        }
        set
        {
            OnStateChange?.Invoke(_state,value);
            _state = value;
        }
    }

    public delegate void StateChangeEvent(EnemyState oldState, EnemyState newState);

    public StateChangeEvent OnStateChange;
    void Awake()
    {
        _steps = GetComponent<AudioSource>();
        enemy = GetComponent<NavMeshAgent>();
        
        _playerTarget = GameObject.FindGameObjectWithTag("Player");

        gameObject.tag = "Enemy";

        OnStateChange += HandleStateChange;

        lineOfSightChecker.OnGainSight += HandleGainSight;
        lineOfSightChecker.OnLoseSight += HandleLoseSight;
    }

    private void Update()
    {
        var distanceToPlayer = Vector3.Distance(transform.position, _playerTarget.transform.position);

        if (distanceToPlayer == 0)
            distanceToPlayer += 0.01f;
        
        var intensity = 1 - distanceToPlayer/maxDistance;

        // Calculate the volume based on intensity.
        var volume = Mathf.Lerp(minVolume, maxVolume, intensity);


        // Set the volume.
        _steps.volume = volume;

        // Check if the enemy is close enough to the player to play footsteps.
        _steps.enabled = distanceToPlayer <= footstepDistance;
    }

    private void HandleGainSight(GameObject player)
    {
        state = EnemyState.Chase;
    }
    
    private void HandleLoseSight(GameObject player)
    {
        state = defaultState;
    }


    private void OnDisable()
    {
        _state = defaultState;
    }


    public void Spawn()
    {
        OnStateChange?.Invoke(EnemyState.Spawn, defaultState);
    }
    /*
    void Update()
    {
        if (isFollowingPlayer)
        {
            // Follow the player
            enemy.SetDestination(_playerTarget.transform.position);

            // Check if it's time to switch to random movement
            randomMoveTimer += Time.deltaTime;
            if (randomMoveTimer >= randomMoveDuration)
            {
                //StartRandomMovement();
            }
        }
        else
        {
            // Move randomly
            enemy.SetDestination(randomMoveTarget);

            // Check if it's time to switch back to following the player
            if (enemy.remainingDistance <= enemy.stoppingDistance)
            {
                //StartFollowingPlayer();
            }
        }
    }
    void StartRandomMovement()
    {
        // Disable following the player
        isFollowingPlayer = false;

        // Generate a random target for random movement
        float randomX = Random.Range(-10f, 10f); // Adjust the range as needed
        float randomZ = Random.Range(-10f, 10f); // Adjust the range as needed
        randomMoveTarget = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        // Reset the timer
        randomMoveTimer = 0f;
    }

    void StartFollowingPlayer()
    {
        // Enable following the player
        isFollowingPlayer=true;
    }
    */

 
    

    private void HandleStateChange(EnemyState oldState, EnemyState newState)
    {
        if (oldState != newState)
        {
            if (_followCoroutine != null)
            {
                StopCoroutine(_followCoroutine);
            }

            if (oldState == EnemyState.Idle)
            {
                enemy.speed /= idleMovementMultiplier;
            }

            switch (newState)
            {
                case EnemyState.Idle:
                    _screamed = false;
                    _followCoroutine = StartCoroutine(DoIdleMotion());
                    break;
                case EnemyState.Chase:
                    _followCoroutine = StartCoroutine(FollowTarget());
                    break;
            }
        }
    }

    private IEnumerator DoIdleMotion()
    {
        var wait = new WaitForSeconds(updateRate
);

        enemy.speed *= idleMovementMultiplier;

        while (true)
        {
            if (!enemy.enabled || !enemy.isOnNavMesh)
            {
                yield return wait;
            }
            
            else if (enemy.remainingDistance <= enemy.stoppingDistance)
            {
                var point = Random.insideUnitCircle * idleLocationRadius;
                NavMeshHit hit;

                if (NavMesh.SamplePosition(enemy.transform.position + new Vector3(point.x, 0, point.y), out hit, 2f, enemy.areaMask))
                {
                    enemy.SetDestination(hit.position);
                }
            }

            yield return wait;
        }
    }

    private IEnumerator FollowTarget()
    {
        var wait = new WaitForSeconds(updateRate
);

        while (gameObject.activeSelf)
        {
            if (enemy.enabled)
            {
                enemy.SetDestination(_playerTarget.transform.position);

                if (!_screamed)
                {
                    attackScream.Play();

                    _screamed = true;
                }
            }

            yield return wait;
        }
    }
}