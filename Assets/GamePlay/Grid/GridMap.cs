using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMap
{
    public class Grid{
    }
    public Grid[,] map;

    public GridMap(int width,int height)
    {
        map = new Grid[width,height];
    }
}
