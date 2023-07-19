using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//让战斗开始的脚本
public class FightStart : MonoBehaviour
{
    private List<GameObject> ballList = PlaceSoliderScript.ballGameObjectList;
    private GameObject PlaceSoldierUI;

    public void Start()
    {
        PlaceSoldierUI = GameObject.Find("PlaceSoldierUI");
    }
    public void FigthStart() //让战斗开始的函数
    {
        foreach (GameObject ball in ballList)
        {
            ball.GetComponent<BallAi>().fsm.SwitchState(StateType.Move);
            PlaceSoldierUI.SetActive(false);
        }
    }
}
