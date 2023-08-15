using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//让战斗开始的脚本
public class FightStart : MonoBehaviour
{
    private List<GameObject> ballList;
    public List<GameObject> closeUI;  //需要关闭的UI集合
    public void Start()
    {
        ballList = BallList.instance.ballGameObjectList;
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
        }
    }
}
