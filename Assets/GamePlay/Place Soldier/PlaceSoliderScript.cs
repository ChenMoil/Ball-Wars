using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using System;

public class PlaceSoliderScript : MonoBehaviour
{
    //放置士兵功能
    [SerializeField] int leftCurrentCoin;   //当前金钱
    [SerializeField] int rightCurrentCoin;
    [SerializeField] Material outlineMat;   //描边材质
    int leftNumberOfSoldiers;         //士兵数量
    int rightNumberOfSoldiers;
    public bool isFree; //是否开启自由模式
    [SerializeField] Rigidbody2D cameraFollow;   //摄像机
    [NonSerialized] public bool isStart; //战斗是否开始
    SummonBall ball;     //当前选择小球
    public GameObject ballListGameObject;
    bool preIsPointerOverUI;//前一帧是否触摸在屏幕上
    public SummonBall Ball{get{return ball;}}
    [SerializeField] Text leftTextCoin;   //显示金钱数量的Ui
    [SerializeField] Text leftTextSolider;   //显示小球数量的Ui
    [SerializeField] Text rightTextCoin;   //显示金钱数量的Ui
    [SerializeField] Text rightTextSolider;   //显示小球数量的Ui

    float mapPos;  //地图中心线位置

    //显示兵种选择界面
    [SerializeField] GameObject gridPrefab; //兵种单位预制体
    [SerializeField] GameObject soldierContent; //兵种单位父物体
    public List<SummonBall> ballList = new List<SummonBall>();
    void Awake()
    {
        if(!isFree)
        leftTextCoin.text=leftCurrentCoin.ToString();   //同步ui显示
        UpdateUI();

        var maps = GameObject.FindGameObjectsWithTag("Map"); //初始化地图位置
        if (maps.Length!=1)
        {
            Debug.LogError("地图不存在或有复数个");
        }
        mapPos = maps[0].transform.position.x;
    }
    private void Start()
    {
        //var balls = GameObject.FindGameObjectsWithTag("BallSoldier");  //把关卡自带的小球加入列表中
        //foreach (var ball in balls)
        //{
        //    BallList.instance.ballGameObjectList.Add(ball);
        //    ball.GetComponent<BallAi>().ballBlackBoard.ballFaction = BallBlackBoard.Faction.Right;
        //}
    }
    void Update(){
        if (Input.touchCount > 0)
        {
            cameraFollow.velocity = -Input.GetTouch(0).deltaPosition;          //摄像机移动
            //Debug.Log(EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId));
            if (ball != null && (isFree || leftCurrentCoin >= ball.coin) && Input.GetTouch(0).deltaPosition == Vector2.zero && !isStart)
            {//放置小球
                if (Input.GetTouch(0).phase == TouchPhase.Ended && !preIsPointerOverUI)
                {
                    //Debug.Log("开始调试"+Input.GetTouch(0).position);
                    Vector3 touchPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position) + 60 * Vector3.forward;
                    if (touchPos.x < mapPos)
                    {
                        GameObject newBall = Instantiate(ball.ball, touchPos, Quaternion.identity);
                        newBall.transform.parent = ballListGameObject.transform;  //将新生成的小球挂载到 BallList 物体上
                        newBall.GetComponent<BallAi>().ballBlackBoard.ballFaction = BallBlackBoard.Faction.Left; //给小球加上阵营
                        newBall.GetComponent<SpriteRenderer>().material=outlineMat;
                        newBall.GetComponent<SpriteRenderer>().material.SetColor("_lineColor", Color.green);

                        PlaceSoldierToLeft(ball.coin);
                    }
                    else if (isFree && touchPos.x>mapPos)
                    {
                        GameObject newBall = Instantiate(ball.ball, touchPos, Quaternion.identity);

                        newBall.transform.parent = ballListGameObject.transform;  //将新生成的小球挂载到 BallList 物体上
                        newBall.GetComponent<BallAi>().ballBlackBoard.ballFaction = BallBlackBoard.Faction.Right; //给小球加上阵营
                        newBall.GetComponent<SpriteRenderer>().material = outlineMat;
                        newBall.GetComponent<SpriteRenderer>().material.SetColor("_lineColor", Color.red);

                        PlaceSoldierToRight(ball.coin);
                    }
                }
            }
            preIsPointerOverUI = EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);
        }
        else
        {
            cameraFollow.velocity = Vector2.zero;
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
        if(isFree) 
        leftCurrentCoin+=coin;
        else leftCurrentCoin-=coin;
        leftTextCoin.text = leftCurrentCoin.ToString();
        leftNumberOfSoldiers++;
        leftTextSolider.text = leftNumberOfSoldiers.ToString();
    }
    void PlaceSoldierToRight(int coin)   //在右边放置士兵
    {
        if(isFree)
        rightCurrentCoin += coin;
        else rightCurrentCoin -= coin;
        if(rightTextCoin!=null)
        rightTextCoin.text = rightCurrentCoin.ToString();
        rightNumberOfSoldiers++;
        if(rightTextSolider!=null)
        rightTextSolider.text = rightNumberOfSoldiers.ToString();
    }
    void InsertSoldierToUI(SummonBall newBall){
       GridSoldierScript grid= Instantiate(gridPrefab, soldierContent.transform).GetComponent<GridSoldierScript>();
       grid.Image.sprite = newBall.ballImage;
       grid.Text.text = newBall.coin.ToString();
       grid.Name.text=newBall.name;
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
