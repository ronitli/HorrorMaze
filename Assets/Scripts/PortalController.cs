using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalController : MonoBehaviour
{

    public bool randomMode;
    public MazeBuilder mazeBuilder;

    private List<PortalData> portals;

    public static PortalController instance{get; private set;}

    private HudController hudController;

    private void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        hudController = FindAnyObjectByType<HudController>();
        portals = mazeBuilder.portalSpawnPoints;
        portals.ForEach(p => p.SetHUDController(hudController));
    }

    public Vector3 GetDestenation(PortalData data)
    {
        List<PortalData> datas = new List<PortalData>(portals);
        datas.Remove(data);
        return datas[Random.Range(0,datas.Count)].spawnPoint;
    }
}
