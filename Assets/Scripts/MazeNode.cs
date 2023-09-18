using UnityEngine;

public class MazeNode : MonoBehaviour
{
    [SerializeField] protected GameObject[] walls;
    [SerializeField] MeshRenderer floor;

    public void RemoveWall(int wallToRemove)
    {
        walls[wallToRemove].gameObject.SetActive(false);
    }
}
