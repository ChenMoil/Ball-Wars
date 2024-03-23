using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    DrawGridScript gridManager;
    BuildBuildingsScript buildingsScript;
    public GameObject prefab;
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
        if (Input.touchCount > 0 && Input.GetTouch(0).phase==TouchPhase.Ended)
            buildingsScript.Build(prefab,Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position));

    }
}
