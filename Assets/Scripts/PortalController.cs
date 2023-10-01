using System.Collections.Generic;
using UnityEngine;

public class PortalController : MonoBehaviour
{

    public bool randomMode;
    public MazeBuilder mazeBuilder;

    private List<PortalData> _portals;

    public static PortalController instance{get; private set;}

    private HudController _hudController;

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
        _hudController = FindAnyObjectByType<HudController>();
        _portals = mazeBuilder.portalSpawnPoints;
        _portals.ForEach(p => p.SetHUDController(_hudController));
    }

    public Vector3 GetDestenation(PortalData data)
    {
        List<PortalData> datas = new List<PortalData>(_portals);
        datas.Remove(data);
        return datas[Random.Range(0,datas.Count)].spawnPoint;
    }
}
