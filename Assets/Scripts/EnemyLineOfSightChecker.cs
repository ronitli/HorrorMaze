using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent((typeof(SphereCollider)))]
public class EnemyLineOfSightChecker : MonoBehaviour
{
    public SphereCollider Collider;
    public float FieldOfView = 180f;
    public LayerMask LineOfSightLayers;
    
    public delegate void GainSightEvent(GameObject player);
    public GainSightEvent OnGainSight;
    public delegate void LoseSightEvent(GameObject player);
    public LoseSightEvent OnLoseSight;

    private Coroutine checkForLineOfSightCoroutine;

    private void Awake()
    {
        Collider = GetComponent<SphereCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject player;
        if (other.tag == "Player")
        {
            player = other.gameObject;
            if (!CheckLineOfSight(player))
            {
                checkForLineOfSightCoroutine = StartCoroutine(CheckForLineOfSight(player));
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        GameObject player;
        if (other.tag == "Player")
        {
            player = other.gameObject;
            OnLoseSight?.Invoke(player);
            gameObject.GetComponent<SphereCollider>().radius = 5;
            if (checkForLineOfSightCoroutine != null)
            {
                StopCoroutine(checkForLineOfSightCoroutine);
            }
        }
    }

    private bool CheckLineOfSight(GameObject player)
    {
        var direction = player.transform.position - transform.position;

        if (Vector3.Dot(transform.forward, direction) >= Mathf.Cos(FieldOfView))
        {
            OnGainSight?.Invoke(player);
            gameObject.GetComponent<SphereCollider>().radius = 8;
            return true;
        }
        

        return false;
    }

    private IEnumerator CheckForLineOfSight(GameObject player)
    {
        var wait = new WaitForSeconds(0.1f);

        while (!CheckLineOfSight(player))
        {
            yield return wait;
        }

    }
}
