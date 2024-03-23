using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMap
{
    public class Grid{
        public bool isPlaced;  //是否被占用
    }
    public Grid[,] map;
    int width;
    int height;

    public GridMap(int width,int height)
    {
        map = new Grid[width,height];
        this.width = width;
        this.height = height;
    }
    public bool IsEnoughSpace(Vector2Int pos,int width,int height)        //判断建筑物是否有足够的空间放置
    {
        if (pos.x + this.width > width || pos.y + this.height > height) return false;
        for(int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                if (!map[pos.y + i, pos.x + j].isPlaced) return false;
            }
        }
        return true;
    }
}
