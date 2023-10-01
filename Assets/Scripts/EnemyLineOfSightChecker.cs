using System.Collections;
using UnityEngine;
[RequireComponent((typeof(SphereCollider)))]
public class EnemyLineOfSightChecker : MonoBehaviour
{
    public SphereCollider Collider;
    public float fieldOfView = 180f;
    public LayerMask lineOfSightLayers;
    
    public delegate void GainSightEvent(GameObject player);
    public GainSightEvent OnGainSight;
    public delegate void LoseSightEvent(GameObject player);
    public LoseSightEvent OnLoseSight;

    private Coroutine _checkForLineOfSightCoroutine;

    private void Awake()
    {
        Collider = GetComponent<SphereCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var player = other.gameObject;
            if (!CheckLineOfSight(player))
            {
                _checkForLineOfSightCoroutine = StartCoroutine(CheckForLineOfSight(player));
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var player = other.gameObject;
            OnLoseSight?.Invoke(player);
            gameObject.GetComponent<SphereCollider>().radius = 5;
            if (_checkForLineOfSightCoroutine != null)
            {
                StopCoroutine(_checkForLineOfSightCoroutine);
            }
        }
    }

    private bool CheckLineOfSight(GameObject player)
    {
        var direction = player.transform.position - transform.position;

        if (Vector3.Dot(transform.forward, direction) >= Mathf.Cos(fieldOfView))
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
