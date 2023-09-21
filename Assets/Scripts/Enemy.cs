using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public NavMeshAgent enemy;
    private GameObject playerTarget;

    // Control variables for random movement
    private bool isFollowingPlayer = true; // Start by following the player
    private float randomMoveTimer = 0f;
    private float randomMoveDuration = 5f; // Adjust this as needed
    private Vector3 randomMoveTarget; // Target for random movement

    void Awake()
    {
        enemy = GetComponent<NavMeshAgent>();
        playerTarget = GameObject.FindGameObjectWithTag("Player");
    }

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
                StartRandomMovement();
            }
        }
        else
        {
            // Move randomly
            enemy.SetDestination(randomMoveTarget);

            // Check if it's time to switch back to following the player
            if (enemy.remainingDistance <= enemy.stoppingDistance)
            {
                StartFollowingPlayer();
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
}