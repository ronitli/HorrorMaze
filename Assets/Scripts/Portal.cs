using UnityEngine;

public class Portal : MonoBehaviour
{
    private PortalData _portalData; 

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")){
            var spawnPoint = PortalController.instance.GetDestenation(_portalData);
            var cc = other.GetComponent<CharacterController>();
            cc.enabled = false;
            other.transform.position = spawnPoint;
            cc.enabled = true;
            _portalData.hudController.UpdateScore(_portalData.hudController.score + 50);
        }
    }

    public void SetPortalData(PortalData data)
    {
        _portalData = data;
    }
}
