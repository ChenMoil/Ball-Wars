using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugButton : MonoBehaviour
{
    public GameObject target;   //需要显示数据的物体

    public Text text;           //文本
    public enum DataType
    {
        Acceleration,    //加速度
        AngularVelocity, //加角速度
    }

    public DataType dataType;
    // Update is called once per frame
    void Update()
    {
        if (dataType == DataType.Acceleration)
        {
            text.text = "加速度:" + BallList.instance.ballBlackBoards[target].acceleration;
        }
        else if (dataType == DataType.AngularVelocity)
        {
            text.text = "加角速度:" + BallList.instance.ballBlackBoards[target].addAngularVelocity;
        }
    }
    public void AddValue()
    {
        if (dataType == DataType.Acceleration)
        {
            BallList.instance.ballBlackBoards[target].acceleration += 0.5f;
        }
        else if (dataType == DataType.AngularVelocity)
        {
            BallList.instance.ballBlackBoards[target].addAngularVelocity += 0.1f;
        }
    }

    public void ReduceValue()
    {
        if (dataType == DataType.Acceleration)
        {
            BallList.instance.ballBlackBoards[target].acceleration -= 0.5f;
        }
        else if (dataType == DataType.AngularVelocity)
        {
            BallList.instance.ballBlackBoards[target].addAngularVelocity -= 0.1f;
        }
    }
}
