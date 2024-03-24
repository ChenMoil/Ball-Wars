using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Test : MonoBehaviour
{
    DrawGridScript gridManager;
    BuildBuildingsScript buildingsScript;
    public Building building;
    bool isOverUI;
    void Start()
    {
        Grid grid = new Grid();
        //grid.Draw();
        gridManager = GameObject.Find("GridManager").GetComponent<DrawGridScript>();
        buildingsScript = GameObject.Find("BuildManager").GetComponent<BuildBuildingsScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0 )
        {
            if(Input.GetTouch(0).phase == TouchPhase.Ended && !isOverUI)
                buildingsScript.Build(building, Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position));
            Debug.Log(EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId));
            isOverUI = EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);
        }
        
    }
}
