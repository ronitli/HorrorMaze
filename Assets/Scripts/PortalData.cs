using UnityEngine;

public class PortalData {
    public PortalData(Transform relatedWall, Vector3 spawnPoint)
    {
        this.relatedWall = relatedWall;
        this.spawnPoint = spawnPoint;
    }
    public Transform relatedWall{get;set;}
    public Vector3 spawnPoint{get;set;}

    public HudController hudController{get; private set;}

    public void SetHUDController(HudController controller)
    {
        this.hudController = controller;
    }
}