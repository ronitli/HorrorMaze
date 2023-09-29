using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    private PortalData portalData; 

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")){
            Vector3 spawnPoint = PortalController.instance.GetDestenation(portalData);
            CharacterController cc = other.GetComponent<CharacterController>();
            cc.enabled = false;
            other.transform.position = spawnPoint;
            cc.enabled = true;
        }
    }

    public void SetPortalData(PortalData data)
    {
        portalData = data;
    }
}
