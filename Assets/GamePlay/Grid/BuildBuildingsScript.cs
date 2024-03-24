using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildBuildingsScript : MonoBehaviour
{
    Transform buildingParent;
    bool isBuilding;   //是否正在建造
    GameObject building; //正在建造的物体
    Color trueColor = new(0, 255, 0, 96);
    private void Start()
    {
        transform.position = GameObject.FindGameObjectWithTag("Map").transform.position;
        buildingParent = transform.Find("BuildingList");
    }
    public void Build(GameObject prefab,Vector2 pos)
    {
        Vector2 grid=DrawGridScript.instance.GetTouchGrid(pos);
        pos = DrawGridScript.instance.GetGridPos(grid);
        GameObject building = Instantiate(prefab,buildingParent);
        Vector2 gridSize = DrawGridScript.instance.gridSize;
        Vector2 buildingSize = building.GetComponent<SpriteRenderer>().sprite.bounds.size;
        building.transform.localPosition = pos;        
        building.transform.localScale=new Vector2(gridSize.x/buildingSize.x,gridSize.y/buildingSize.y);

        //if(DrawGridScript.instance.map.IsEnoughSpace(grid,))
        //isBuilding = true;
        //this.building=building;
         
    }
}
