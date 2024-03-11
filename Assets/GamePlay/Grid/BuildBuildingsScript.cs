using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildBuildingsScript : MonoBehaviour
{
    Transform buildingParent;
    private void Start()
    {
        transform.position = GameObject.FindGameObjectWithTag("Map").transform.position;
        buildingParent = transform.Find("BuildingList");
    }
    public void Build(GameObject prefab,Vector2 pos)
    {
        pos = DrawGridScript.instance.TargetGrid(pos);
        GameObject building = Instantiate(prefab,buildingParent);
        Vector2 gridSize = DrawGridScript.instance.gridSize;
        Vector2 buildingSize = building.GetComponent<SpriteRenderer>().sprite.bounds.size;
        building.transform.localPosition = pos;        
        building.transform.localScale=new Vector2(gridSize.x/buildingSize.x,gridSize.y/buildingSize.y);
    }
}
