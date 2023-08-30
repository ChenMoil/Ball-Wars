using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//让战斗开始的脚本
public class FightStart : MonoBehaviour
{
    private List<GameObject> ballList;
    public List<GameObject> closeUI;  //需要关闭的UI集合
    private PlaceSoliderScript place;  //放置士兵的脚本
    public void Start()
    {
        ballList = BallList.instance.ballGameObjectList;

        place = GameObject.Find("Place soldier").GetComponent<PlaceSoliderScript>();
        GameObject square = GameObject.Find("Square");
        if (!closeUI.Contains(square))
        {
            closeUI.Add(square);
        }
    }
    public void FigthStart() //让战斗开始的函数
    {
        Debug.Log("战斗开始");
        foreach (GameObject gameObject in closeUI)
        {
            gameObject.SetActive(false);
        }
        foreach (GameObject ball in ballList)
        {
            ball.GetComponent<BallAi>().fsm.SwitchState(StateType.Move);
            if (ball.GetComponent<BallAi>().ballBlackBoard.ballFaction == BallBlackBoard.Faction.Left)
            {
                BallList.instance.leftBallNum++;
            }
            else
            {
                BallList.instance.rightBallNum++;
            }
        }
        place.isStart = true; //战斗开始
    }
}
