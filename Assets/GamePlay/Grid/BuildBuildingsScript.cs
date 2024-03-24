using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildBuildingsScript : MonoBehaviour
{
    Transform buildingParent;
    bool isBuilding;   //是否正在建造
    bool isCapableofBuild; //是否能够建造
    GameObject building; //正在建造的物体
    Color trueColor = new(0, 255, 0, 96);
    Color falseColor = new Color(255, 0, 0, 96);
    [SerializeField] Transform UITransform;
    [SerializeField] GameObject confirmButtonPrefab;
    [SerializeField] GameObject cancelButtonPrefab;
    GameObject confirmButton;
    GameObject cancelButton;
    Building currentBuildingInfo;
    private void Start()
    {
        transform.position = GameObject.FindGameObjectWithTag("Map").transform.position;
        buildingParent = transform.Find("BuildingList");
        UITransform = transform.Find("Canvas");
    }
    public void Build(Building targetBuilding,Vector2 pos)
    {
        if (isBuilding)
        {
            Destroy(building);
            isBuilding = false;
            building = null;

            Destroy(confirmButton);
            Destroy(cancelButton);
        }

        currentBuildingInfo=targetBuilding;

        Vector2Int grid=DrawGridScript.instance.GetTouchGrid(pos);
        pos = DrawGridScript.instance.GetGridPos(grid);
        GameObject tempBuilding = Instantiate(targetBuilding.prefab,buildingParent);
        Vector2 gridSize = DrawGridScript.instance.gridSize;
        Vector2 buildingSize = tempBuilding.GetComponent<SpriteRenderer>().sprite.bounds.size;
        tempBuilding.transform.localPosition = pos;        
        tempBuilding.transform.localScale=new Vector2(gridSize.x/buildingSize.x,gridSize.y/buildingSize.y);

        if (DrawGridScript.instance.map.IsEnoughSpace(grid, targetBuilding.width, targetBuilding.height))
        {
            tempBuilding.GetComponent<SpriteRenderer>().color = trueColor;
            isCapableofBuild = true;
        }
        else
        {
            tempBuilding.GetComponent<SpriteRenderer>().color = falseColor;
            isCapableofBuild = false;
        }
        //禁用建筑脚本
        isBuilding = true;
        building=tempBuilding;
        ShowTipsButtons(grid);
    }
    public void ConfirmBuild()               //确认建造
    {
        if (!isBuilding || building==null)
        {
            Debug.LogError("未建造却确认建造");
            return;
        }
        building.GetComponent<SpriteRenderer>().color = Color.white;
        DrawGridScript.instance.map.Place(DrawGridScript.instance.GetTouchGrid(building.transform.position), currentBuildingInfo.width, currentBuildingInfo.height);
        //启用建筑脚本
        isBuilding = false;
        building=null;

        Destroy(confirmButton);
        Destroy(cancelButton);
    }
    public void CancelBuild()               //取消建造
    {
        if (!isBuilding || building == null)
        {
            Debug.LogError("未建造却取消建造");
            return;
        }
        Destroy(building);
        isBuilding = false;
        building = null;

        Destroy(confirmButton);
        Destroy(cancelButton);
    }
    void ShowTipsButtons(Vector2Int pos)
    {
        bool buttonsDirection;           //按钮的方向
        if (DrawGridScript.instance.isInCamera(pos))
            buttonsDirection = true;
        else 
            buttonsDirection = false;
        pos.x += buttonsDirection ? 1 : -1;
        Vector2 gridSize = DrawGridScript.instance.gridSize;
        
        Vector2 confirmButtonPos = Camera.main.WorldToScreenPoint(DrawGridScript.instance.GetGridPos(pos) + new Vector2(0, gridSize.y) * 0.3f);
        Vector2 cancelButtonPos = Camera.main.WorldToScreenPoint(DrawGridScript.instance.GetGridPos(pos) + new Vector2(0, -1 * gridSize.y) * 0.7f);
        //对按钮进行缩放处理
        confirmButton = Instantiate(confirmButtonPrefab, UITransform);
        cancelButton = Instantiate(cancelButtonPrefab, UITransform);
        confirmButton.transform.position = confirmButtonPos;
        confirmButton.GetComponent<Button>().onClick.AddListener(ConfirmBuild);
        cancelButton.transform.position = cancelButtonPos;
        cancelButton.GetComponent<Button>().onClick.AddListener(CancelBuild);

        if (!isCapableofBuild)
            confirmButton.GetComponent<Image>().color = new Color(0, 0, 0, 96);
    }
}
