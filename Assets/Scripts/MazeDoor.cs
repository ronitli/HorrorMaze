using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;

public class MazeDoor : MazeNode
{
    public void RotateAndMoveWalls(float y)
    {
        transform.Rotate(0, y, 0);
        GameObject w0 = walls[0];
        GameObject w1 = walls[1];
        GameObject w2 = walls[2];
        GameObject w3 = walls[3];
        if (y==0)
        {
            return;
        }
        else if (y==90)
        {
            walls[3] = w0;
            walls[2] = w1;
            walls[1] = w3;
            walls[0] = w2;
        } else if (y == 180) {
            walls[1] = w0;
            walls[0] = w1;
            walls[3] = w2;
            walls[2] = w3;
        } else if (y == 270) {
            walls[3] = w0;
            walls[2] = w1;
            walls[0] = w2;
            walls[1] = w3;
        }
    }


}
