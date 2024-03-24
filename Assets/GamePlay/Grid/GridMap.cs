using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class GridMap
{
    public class Grid{
        public bool isPlaced;  //是否被占用
        public Grid()
        {
            isPlaced = false;
        }
    }
    public Grid[,] map;
    int width;
    int height;

    public GridMap(int width,int height)
    {
       // Debug.Log(width.ToString() + " " + height.ToString());  
        map = new Grid[height, width];
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                map[i,j] = new Grid();
            }
        }
        this.width = width;
        this.height = height;
    }
    //根据中心线的位置改变格子阵营的构造函数
    public bool IsEnoughSpace(Vector2Int pos,int width,int height)        //判断建筑物是否有足够的空间放置
    {
        if (pos.x + width > this.width || pos.y + height > this.height) return false;
        //Debug.Log(pos.ToString() + width.ToString() + height.ToString() + "\n" + map[0,0].isPlaced);
        for(int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                if (map[pos.y + i, pos.x + j].isPlaced) return false;
            }
        }
        return true;
    }
    public void Place(Vector2Int pos, int width, int height)            //占用空间
    {
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                map[pos.y + i, pos.x + j].isPlaced = true;
            }
        }
    }          
}
