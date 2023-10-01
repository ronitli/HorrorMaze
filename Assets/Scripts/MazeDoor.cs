using UnityEngine;

public class MazeDoor : MazeNode
{
    public void RotateAndMoveWalls(float y)
    {
        GameObject[] wallsTemp = new GameObject[walls.Length];
        wallsTemp[0] = walls[0];
        wallsTemp[1] = walls[2];
        wallsTemp[2] = walls[1];
        wallsTemp[3] = walls[3];
        y %= 360;

        transform.Rotate(0, y, 0);

        int firstWallIndex = (int)y/90%4;
                                                       //   0     90    180    270
        walls[0] = wallsTemp[firstWallIndex];          //   0     1     2      3
        walls[2] = wallsTemp[(firstWallIndex+1)%4];    //   1     2     3      0
        walls[1] = wallsTemp[(firstWallIndex+2)%4];    //   2     3     0      1
        walls[3] = wallsTemp[(firstWallIndex+3)%4];    //   3     0     1      2

    }


}
