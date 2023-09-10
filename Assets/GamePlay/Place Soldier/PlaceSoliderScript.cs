using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlaceSoliderScript : MonoBehaviour
{
    //放置士兵功能
    [SerializeField] public int leftCurrentCoin;   //当前金钱
    [SerializeField] int rightCurrentCoin;
    [SerializeField] Material outlineMat;   //描边材质
    public int leftNumberOfSoldiers;         //士兵数量
    int rightNumberOfSoldiers;
    [SerializeField] bool isFree; //是否开启自由模式
    Rigidbody2D cameraFollow;   //摄像机
    BoxCollider2D cameraCollider; //摄像机碰撞体
    [NonSerialized] public bool isStart; //战斗是否开始
    SummonBall ball;     //当前选择小球
    private GameObject ballListGameObject;
    bool preIsPointerOverUI;//前一帧是否触摸在屏幕上
    [SerializeField] float longPressTime; //开始放置士兵的长按时间
    [SerializeField] float longPressPlaceSoliderTime;//长按时放置士兵的间隔时间
    float longPressTimer;   //长按时间
    bool isLongPressPlace;  //长按时放小球
    public SummonBall Ball { get { return ball; } }
    [SerializeField] Text leftTextCoin;   //显示金钱数量的Ui
    [SerializeField] Text leftTextSolider;   //显示小球数量的Ui
    [SerializeField] Text rightTextCoin;   //显示金钱数量的Ui
    [SerializeField] Text rightTextSolider;   //显示小球数量的Ui

    float mapPos;  //地图中心线位置

    //显示兵种选择界面
    [SerializeField] GameObject gridPrefab; //兵种单位预制体
    [SerializeField] GameObject soldierContent; //兵种单位父物体
    public List<SummonBall> ballList = new List<SummonBall>();
    List<SummonBall> currentBallList;//当前小球列表
    void Awake()
    {
        //初始化种类列表
        ChangeCategory(SummonBall.Category.all);
        ballListGameObject = GameObject.Find("BallList");
        if (!isFree)
            leftTextCoin.text = leftCurrentCoin.ToString();   //同步ui显示
        UpdateUI();

        var maps = GameObject.FindGameObjectsWithTag("Map"); //初始化地图位置
        if (maps.Length != 1)
        {
            Debug.LogError("地图不存在或有复数个");
        }
        mapPos = maps[0].transform.Find("Square").position.x;

        //初始化摄像机
        GameObject camera = GameObject.Find("Camera Follow");
        cameraCollider = camera.GetComponent<BoxCollider2D>();
        cameraFollow = camera.GetComponent<Rigidbody2D>();
        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector3(0, 0));// 获取摄像机的视野范围（左下角和右上角的视口坐标）
        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector3(1, 1));
       
        // 设置碰撞体的中心和大小为视野范围
        cameraCollider.size = max - min;

      
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
    void Update()
    {
        if (Input.touchCount > 0)
        {
            cameraFollow.velocity = -Input.GetTouch(0).deltaPosition;          //摄像机移动
            if (Input.GetTouch(0).deltaPosition != Vector2.zero)
            {
                longPressTimer = Mathf.Infinity;
            }
            if (ball != null && (isFree || leftCurrentCoin >= ball.coin) && Input.GetTouch(0).deltaPosition == Vector2.zero && !OtherButton.instance.isStart)
            {//放置小球
                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    longPressTimer = Time.time + longPressTime + longPressPlaceSoliderTime;
                }
                else if (Input.GetTouch(0).phase == TouchPhase.Stationary && Time.time > longPressTimer)
                {
                    longPressTimer += longPressPlaceSoliderTime;
                    isLongPressPlace = true;
                }
                if ((Input.GetTouch(0).phase == TouchPhase.Ended && !preIsPointerOverUI) || isLongPressPlace)
                {
                    isLongPressPlace = false;
                    Vector3 touchPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position) + 60 * Vector3.forward;
                    if (touchPos.x < mapPos)
                    {
                        GameObject newBall = Instantiate(ball.ball, touchPos, Quaternion.identity);
                        newBall.transform.parent = ballListGameObject.transform;  //将新生成的小球挂载到 BallList 物体上
                        newBall.GetComponent<BallAi>().ballBlackBoard.ballFaction = BallBlackBoard.Faction.Left; //给小球加上阵营
                        newBall.GetComponent<SpriteRenderer>().material = outlineMat;
                        newBall.GetComponent<SpriteRenderer>().material.SetColor("_lineColor", Color.green);

                        PlaceSoldierToLeft(ball.coin);
                    }
                    else if (isFree && touchPos.x > mapPos)
                    {
                        GameObject newBall = Instantiate(ball.ball, touchPos, Quaternion.identity);

                        newBall.transform.parent = ballListGameObject.transform;  //将新生成的小球挂载到 BallList 物体上
                        newBall.GetComponent<BallAi>().ballBlackBoard.ballFaction = BallBlackBoard.Faction.Right; //给小球加上阵营
                        newBall.GetComponent<SpriteRenderer>().material = outlineMat;
                        newBall.GetComponent<SpriteRenderer>().material.SetColor("_lineColor", Color.red);

                        PlaceSoldierToRight(ball.coin);
                    }
                    else if (!isFree && touchPos.x > mapPos && !OtherButton.instance.isStart)
                    {
                        SignUI.instance.DisplayText("无法在敌方地盘放置士兵", 1f, Color.white);
                    }
                }
            }
            else if (ball != null && leftCurrentCoin < ball.coin && !OtherButton.instance.isStart)
            {
                SignUI.instance.DisplayText("缺少金币，无法放置士兵", 1f, Color.white);
            }
            preIsPointerOverUI = EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);
        }
        else
        {
            cameraFollow.velocity = Vector2.zero;
            longPressTimer = Mathf.Infinity;
        }


    }
    public void ChangeBall(SummonBall newBall)
    {    //改变当前选择小球
        ball = newBall;
    }
    public void ChangeBall()  //重置当前选择小球
    {
        ball = null;
    }
    void PlaceSoldierToLeft(int coin)   //在左边放置士兵
    {
        if (isFree)
            leftCurrentCoin += coin;
        else leftCurrentCoin -= coin;
        leftTextCoin.text = leftCurrentCoin.ToString();
        leftNumberOfSoldiers++;
        leftTextSolider.text = leftNumberOfSoldiers.ToString();
    }
    void PlaceSoldierToRight(int coin)   //在右边放置士兵
    {
        if (isFree)
            rightCurrentCoin += coin;
        else rightCurrentCoin -= coin;
        if (rightTextCoin != null)
            rightTextCoin.text = rightCurrentCoin.ToString();
        rightNumberOfSoldiers++;
        if (rightTextSolider != null)
            rightTextSolider.text = rightNumberOfSoldiers.ToString();
    }
    void InsertSoldierToUI(SummonBall newBall)
    {
        GridSoldierScript grid = Instantiate(gridPrefab, soldierContent.transform).GetComponent<GridSoldierScript>();
        grid.Image.sprite = newBall.ballImage;
        grid.Text.text = newBall.coin.ToString();
        grid.Name.text = newBall.name;
        grid.ball = newBall;
        grid.Toggle.group = soldierContent.GetComponent<ToggleGroup>();
    }
    void InsertCategoryToUI()
    {

    }
    public void RefreshText()
    {
        leftTextCoin.text = leftCurrentCoin.ToString();
        leftTextSolider.text = leftNumberOfSoldiers.ToString();
    }
    public void UpdateUI()
    {
        for (int i = soldierContent.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(soldierContent.transform.GetChild(i).gameObject);
        }
        for (int i = 0; i < currentBallList.Count; i++)
        {
            if (currentBallList[i] != null)
            {
                InsertSoldierToUI(currentBallList[i]);
            }
        }
    }
    public void ChangeCategory(SummonBall.Category category)
    {
        if (category==SummonBall.Category.all)
        {
            currentBallList=ballList;
        }
        else
        {
            currentBallList.Clear();
            foreach (var ball in ballList)
            {
                if (ball.category==category)
                {
                    currentBallList.Add(ball);
                }
            }
        }
        UpdateUI();
    }
}
