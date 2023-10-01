using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
public class MazeBuilder : MonoBehaviour
{
    [SerializeField] MazeNode nodePrefab;
    [SerializeField] MazeNode doorNode;
    [SerializeField] Portal portalPrefab;
    [SerializeField] Vector2Int mazeSize;
    [SerializeField] bool debug;
    
    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject enemyPrefab;

    private Vector3 _playerSpawnPoint;
    private Vector3[] _enemySpawnPoint = new Vector3[2];
    private NavMeshTriangulation _triangulation;

    private int _portalAmount;
    public List<PortalData> portalSpawnPoints {get; private set;}

    public int enemyCounter = 2;

    public List<Transform> undeletedWalls;

    public NavMeshSurface surface;
    private void Start()
    {
        _portalAmount = Mathf.Min(mazeSize.x, mazeSize.y);
        portalSpawnPoints = new List<PortalData>();
        GenerateMazeInstant(mazeSize);
        surface.BuildNavMesh();
        _triangulation = NavMesh.CalculateTriangulation();
        SpawnPlayer();
        if (!debug) SpawnEnemy();
    }

    private void SpawnPlayer()
    {
        var player = Instantiate(playerPrefab, _playerSpawnPoint + new Vector3(0,1f,0), Quaternion.identity, transform);
        player.SetActive(true);
    }

    private void SpawnEnemy()
    {
        for (var i = 0; i < enemyCounter; i++)
        {
            var enemy = Instantiate(enemyPrefab, _enemySpawnPoint[i] + new Vector3(0,1f,0), Quaternion.identity, transform);
            enemy.SetActive(true);
            var enemy1 = enemy.GetComponent<Enemy>();
            enemy1.Triangulation = _triangulation;
            enemy1.Spawn();
        }
    }

    private void GenerateMazeInstant(Vector2Int size)
    {
        var doorSide = Random.Range(0, 4); // 0 for up, 1 down, 2 left, 3 right

        var doorPos = new Vector2Int();
		var yRotation = 0f;
        if (doorSide == 0)
        {
            var rnd = Random.Range(0, size.x);
            doorPos = new Vector2Int(0, rnd);
            yRotation = 270f;
        }
        else if (doorSide == 1)
        {
            var rnd = Random.Range(0, size.x);
            doorPos = new Vector2Int(size.y-1, rnd);
            yRotation = 90f;
        }
        else if (doorSide == 2)
        {
            var rnd = Random.Range(0, size.y);
            doorPos = new Vector2Int(rnd, 0);
            yRotation = 180f;
        }
        else
        {
            var rnd = Random.Range(0, size.y);
            doorPos = new Vector2Int(rnd, size.x-1);
            yRotation = 0f;
        }
        
        var nodes = new List<MazeNode>();
        Debug.Log(doorPos + " side: " + doorSide);
        // Create nodes
        MazeDoor doorNodeMazeNode = null;
        for (var x = 0; x < size.x; x++)
        {
            for (var y = 0; y < size.y; y++)
            {
                MazeNode newNode;
                var cellSize = 10.0f; // Adjust the cell size as needed (e.g., 2.0f for larger cells).
                var nodePos = new Vector3(x * cellSize - ((size.x - 1) * cellSize / 2f), 0, y * cellSize - ((size.y - 1) * cellSize / 2f));
                if (x == size.x / 2 && y == size.y / 2)
                {
                    _playerSpawnPoint = nodePos;
                }
                
                else if (x == size.x / 2 + 2 && y == size.y / 2 - 2)
                {
                    _enemySpawnPoint[0] = nodePos;
                }
                else if (x == size.x / 2 - 2 && y == size.y / 2 + 2)
                {
                    _enemySpawnPoint[1] = nodePos;
                }
                if (x == doorPos.x && y == doorPos.y)
                {
                    newNode = Instantiate(doorNode, nodePos, Quaternion.identity, transform);
                    doorNodeMazeNode = newNode.GetComponent<MazeDoor>();
                    doorNodeMazeNode.RotateAndMoveWalls(yRotation);
                }
                else
                {
                    newNode = Instantiate(nodePrefab, nodePos, Quaternion.identity, transform);
                }
                nodes.Add(newNode);
                
            }
        }

        var currentPath = new List<MazeNode>();
        var completedNodes = new List<MazeNode>();

        // Choose starting node
        currentPath.Add(nodes[Random.Range(0, nodes.Count)]);

        undeletedWalls = new List<Transform>();  

        while (completedNodes.Count < nodes.Count)
        {
            // Check nodes next to the current node
            var possibleNextNodes = new List<int>();
            var possibleDirections = new List<int>();

            var currentNodeIndex = nodes.IndexOf(currentPath[currentPath.Count - 1]);
            var currentNodeX = currentNodeIndex / size.y;
            var currentNodeY = currentNodeIndex % size.y;

            if (currentNodeX < size.x - 1)
            {
                // Check node to the right of the current node
                if (!completedNodes.Contains(nodes[currentNodeIndex + size.y]) &&
                    !currentPath.Contains(nodes[currentNodeIndex + size.y]))
                {
                    possibleDirections.Add(1);
                    possibleNextNodes.Add(currentNodeIndex + size.y);
                }
            }
            if (currentNodeX > 0)
            {
                // Check node to the left of the current node
                if (!completedNodes.Contains(nodes[currentNodeIndex - size.y]) &&
                    !currentPath.Contains(nodes[currentNodeIndex - size.y]))
                {
                    possibleDirections.Add(2);
                    possibleNextNodes.Add(currentNodeIndex - size.y);
                }
            }
            if (currentNodeY < size.y - 1)
            {
                // Check node above the current node
                if (!completedNodes.Contains(nodes[currentNodeIndex + 1]) &&
                    !currentPath.Contains(nodes[currentNodeIndex + 1]))
                {
                    possibleDirections.Add(3);
                    possibleNextNodes.Add(currentNodeIndex + 1);
                }
            }
            if (currentNodeY > 0)
            {
                // Check node below the current node
                if (!completedNodes.Contains(nodes[currentNodeIndex - 1]) &&
                    !currentPath.Contains(nodes[currentNodeIndex - 1]))
                {
                    possibleDirections.Add(4);
                    possibleNextNodes.Add(currentNodeIndex - 1);
                }
            }

            // Choose next node
            if (possibleDirections.Count > 0)
            {
                var chosenDirection = Random.Range(0, possibleDirections.Count);
                var chosenNode = nodes[possibleNextNodes[chosenDirection]];

                switch (possibleDirections[chosenDirection])
                {
                    case 1:
                        chosenNode.RemoveWall(1);
                        currentPath[currentPath.Count - 1].RemoveWall(0);
                        break;
                    case 2:
                        chosenNode.RemoveWall(0);
                        currentPath[currentPath.Count - 1].RemoveWall(1);
                        break;
                    case 3:
                        chosenNode.RemoveWall(3);
                        currentPath[currentPath.Count - 1].RemoveWall(2);
                        break;
                    case 4:
                        chosenNode.RemoveWall(2);
                        currentPath[currentPath.Count - 1].RemoveWall(3);
                        break;
                }

                currentPath.Add(chosenNode);
            }
            else
            {
                completedNodes.Add(currentPath[currentPath.Count - 1]);

                currentPath.RemoveAt(currentPath.Count - 1);
            }

            // for (var i=0;i<4;i++)
            // {
            //     if (nodes[currentNodeIndex].GetWall(i).activeInHierarchy && !nodes[currentNodeIndex].GetWall(i).Equals(doorNodeMazeNode.GetWall(2)))
            //     {   
            //         undeletedWalls.Add(nodes[currentNodeIndex].GetWall(i).transform);    
            //     }
            // }
        }

        
        foreach (var node in nodes)
        {
             for (var j = 0; j < 4 ;j++)
             {
                if (node.GetWall(j).activeInHierarchy && !node.GetWall(j).Equals(doorNodeMazeNode.GetWall(2)))
                {   
                    undeletedWalls.Add(node.GetWall(j).transform);    
                }
             }
        }
    
        for (var i=0;i<_portalAmount
;i++)
        {
            var index = Random.Range(0, undeletedWalls.Count);
            portalSpawnPoints.Add(new PortalData(undeletedWalls[index], undeletedWalls[index].transform.parent.position));
            undeletedWalls.RemoveAt(index);
        }
    
        foreach(var portalData in portalSpawnPoints)
        {
            var pos = portalData.relatedWall.transform.position;
            var center = GetMazeNodeCenter(portalData.relatedWall.parent.gameObject);
            var changeDir = (center-pos).normalized;
            pos += (center-pos)*0.05f;
            pos.y = center.y*0.9f;
            var portal = Instantiate(portalPrefab, 
                pos, 
                Quaternion.LookRotation(changeDir, portalData.relatedWall.up),
                portalData.relatedWall.transform.parent);
            portal.SetPortalData(portalData);
        }
    }


    private Vector3 GetMazeNodeCenter(GameObject node)
    {
        if (node.TryGetComponent(out MazeNode m))
        {
            var res = Vector3.zero;
            for (int i = 0; i < 4; i++)
            {
                res += m.GetWall(i).transform.position;
            }

            return res / 4;
        }

        throw new System.Exception("Node is not a maze node:" + node.name);
    }

    public PortalData GetPortalData(int index)
    {
        return portalSpawnPoints[index];
    }

    public int GetPortalDataCount()
    {
        return portalSpawnPoints.Count;
    }
}
