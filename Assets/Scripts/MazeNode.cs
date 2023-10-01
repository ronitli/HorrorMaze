using UnityEngine;

public class MazeNode : MonoBehaviour
{
    [SerializeField] protected GameObject[] walls;

    public void RemoveWall(int wallToRemove)
    {
        walls[wallToRemove].gameObject.SetActive(false);
    }

    public GameObject GetWall(int wall)
    {
        return walls[wall];
    }
}
