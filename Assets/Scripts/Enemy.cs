using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    
    public float UpdateRate = 0.1f;
    public NavMeshAgent enemy;
    private GameObject playerTarget;
    public NavMeshTriangulation Triangulation;

    public EnemyLineOfSightChecker LineOfSightChecker;

    // Control variables for random movement
    private bool isFollowingPlayer = true; // Start by following the player
    private float randomMoveTimer = 0f;
    private float randomMoveDuration = 5f; // Adjust this as needed
    private Vector3 randomMoveTarget; // Target for random movement

    // Idle movement
    public float IdleLocationRadius = 4f;
    public float IdleMovementMultiplier = 0.5f;

    
    public EnemyState DefaultState = EnemyState.Idle;
    private EnemyState _state;

    private Coroutine followCoroutine;
    public EnemyState State
    {
        get
        {
            return _state;
        }
        set
        {
            onStateChange?.Invoke(_state,value);
            _state = value;
        }
    }

    public delegate void StateChangeEvent(EnemyState oldState, EnemyState newState);

    public StateChangeEvent onStateChange;
    void Awake()
    {
        enemy = GetComponent<NavMeshAgent>();
        playerTarget = GameObject.FindGameObjectWithTag("Player");

        onStateChange += HandleStateChange;

        LineOfSightChecker.OnGainSight += HandleGainSight;
        LineOfSightChecker.OnLoseSight += HandleLoseSight;
    }

    private void HandleGainSight(GameObject player)
    {
        State = EnemyState.Chase;
    }
    
    private void HandleLoseSight(GameObject player)
    {
        State = DefaultState;
    }


    private void OnDisable()
    {
        _state = DefaultState;
    }


    public void Spawn()
    {
        onStateChange?.Invoke(EnemyState.Spawn, DefaultState);
    }
    /*
    void Update()
    {
        if (isFollowingPlayer)
        {
            // Follow the player
            enemy.SetDestination(playerTarget.transform.position);

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
            if (followCoroutine != null)
            {
                StopCoroutine(followCoroutine);
            }

            if (oldState == EnemyState.Idle)
            {
                enemy.speed /= IdleMovementMultiplier;
            }

            switch (newState)
            {
                case EnemyState.Idle:
                    followCoroutine = StartCoroutine(DoIdleMotion());
                    break;
                case EnemyState.Chase:
                    followCoroutine = StartCoroutine(FollowTarget());
                    break;
            }
        }
    }

    private IEnumerator DoIdleMotion()
    {
        var wait = new WaitForSeconds(UpdateRate);

        enemy.speed *= IdleMovementMultiplier;

        while (true)
        {
            if (!enemy.enabled || !enemy.isOnNavMesh)
            {
                yield return wait;
            }
            
            else if (enemy.remainingDistance <= enemy.stoppingDistance)
            {
                var point = Random.insideUnitCircle * IdleLocationRadius;
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
        var wait = new WaitForSeconds(UpdateRate);

        while (gameObject.activeSelf)
        {
            if (enemy.enabled)
            {
                enemy.SetDestination(playerTarget.transform.position);
            }

            yield return wait;
        }
    }
}