using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallList : MonoBehaviour
{
    public static BallList instance;
    public List<GameObject> ballGameObjectList = new List<GameObject>();  //储存所有小球
    public Dictionary<GameObject, BallBlackBoard> ballBlackBoards = new Dictionary<GameObject, BallBlackBoard>(); //将小球物体与他的数据对应
    public int leftBallNum; //左方小球数量
    public int rightBallNum;//右方小球数量
    public enum SceneType
    {
        level,
        Test
    }
    public SceneType sceneType;

    private void Awake()
    {
        instance = this; //每次进入新场景都会更改为当前场景的 BallList
    }
}
