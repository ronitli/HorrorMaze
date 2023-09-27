using UnityEngine;

class PortalData {
    public PortalData(Transform relatedWall, Vector3 spawnPoint)
    {
        this.relatedWall = relatedWall;
        this.spawnPoint = spawnPoint;
    }
    public Transform relatedWall{get;set;}
    public Vector3 spawnPoint{get;set;}
}