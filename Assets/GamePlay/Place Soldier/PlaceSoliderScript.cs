﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlaceSoliderScript : MonoBehaviour
{
    //放置士兵功能
    [SerializeField] int leftCurrentCoin;   //当前金钱
    [SerializeField] int rightCurrentCoin; 
    int leftNumberOfSoldiers;         //士兵数量
    int rightNumberOfSoldiers;
    public bool isFree; //是否开启自由模式
    SummonBall ball;     //当前选择小球
    private GameObject ballListGameObject;
    public SummonBall Ball{get{return ball;}}
    [SerializeField] Text leftTextCoin;   //显示金钱数量的Ui
    [SerializeField] Text leftTextSolider;   //显示小球数量的Ui
    [SerializeField] Text rightTextCoin;   //显示金钱数量的Ui
    [SerializeField] Text rightTextSolider;   //显示小球数量的Ui

    //显示兵种选择界面
    [SerializeField] GameObject gridPrefab; //兵种单位预制体
    [SerializeField] GameObject soldierContent; //兵种单位父物体
    public List<SummonBall> ballList = new List<SummonBall>();
    public static List<GameObject> ballGameObjectList = new List<GameObject>();  //储存所有小球
    void Awake()
    {
        ballListGameObject = GameObject.Find("BallList");
        if(!isFree)
        leftTextCoin.text=leftCurrentCoin.ToString();   //同步ui显示
        UpdateUI();
    }
    void Update(){

        if(ball != null && (isFree || leftCurrentCoin>=ball.coin)){//放置小球
            if (Input.touchCount>0 && Input.GetTouch(0).phase==TouchPhase.Began && !EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
            {
                Debug.Log("开始调试"+Input.GetTouch(0).position);
                Vector3 touchPos=Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position)+10*Vector3.forward;
                GameObject newBall = Instantiate(ball.ball,touchPos , Quaternion.identity);
                newBall.transform.parent = ballListGameObject.transform;  //将新生成的小球挂载到 BallList 物体上
                ballGameObjectList.Add(newBall);                          //将新生成的小球加入容器中
                if (touchPos.x < 0){
                    newBall.GetComponent<BallAi>().ballBlackBoard.ballFaction = BallBlackBoard.Faction.Left; //给小球加上阵营
                    PlaceSoldierToLeft(ball.coin);
                }else{
                    newBall.GetComponent<BallAi>().ballBlackBoard.ballFaction = BallBlackBoard.Faction.Right; //给小球加上阵营
                    PlaceSoldierToRight(ball.coin);
                }
            }
        }
    }
    public void ChangeBall(SummonBall newBall){    //改变当前选择小球
        ball = newBall;
    }
    public void ChangeBall()  //重置当前选择小球
    {
        ball = null;
    }
    void PlaceSoldierToLeft(int coin)   //在左边放置士兵
    {
        leftCurrentCoin+=coin;
        leftTextCoin.text = leftCurrentCoin.ToString();
        leftNumberOfSoldiers++;
        leftTextSolider.text = leftNumberOfSoldiers.ToString();
    }
    void PlaceSoldierToRight(int coin)   //在右边放置士兵
    {
        rightCurrentCoin += coin;
        rightTextCoin.text = rightCurrentCoin.ToString();
        rightNumberOfSoldiers++;
        rightTextSolider.text = rightNumberOfSoldiers.ToString();
    }
    void InsertSoldierToUI(SummonBall newBall){
       GridSoldierScript grid= Instantiate(gridPrefab, soldierContent.transform).GetComponent<GridSoldierScript>();
       grid.Image.sprite = newBall.ballImage;
       grid.Text.text = newBall.coin.ToString();
       grid.ball=newBall;
       grid.Toggle.group=soldierContent.GetComponent<ToggleGroup>();
    }
    public void UpdateUI(){
        for(int i=soldierContent.transform.childCount-1;i>=0;i--){
            Destroy(soldierContent.transform.GetChild(i).gameObject);
        }
        for(int i=0;i<ballList.Count;i++){
                    if(ballList[i]!=null){
                        InsertSoldierToUI(ballList[i]);
                    }
                }
    }
}
