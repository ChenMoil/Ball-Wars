using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlaceSoliderScript : MonoBehaviour
{
    //放置士兵功能
    [SerializeField] int leftCurrentCoin;   //当前金钱
    [SerializeField] int rightCurrentCoin;
    [SerializeField] Rigidbody2D cameraFollow;   //摄像机
    [SerializeField] Material greenOutlineMat;   //绿描边材质
    [SerializeField] Material redOutlineMat;     //红描边材质
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
    void Awake()
    {
        ballListGameObject = GameObject.Find("BallList");
        if(!isFree)
        leftTextCoin.text=leftCurrentCoin.ToString();   //同步ui显示
        UpdateUI();
    }
    void Update(){
        if (Input.touchCount > 0)
        {
            cameraFollow.velocity = -Input.GetTouch(0).deltaPosition;
            //Debug.Log(-Input.GetTouch(0).deltaPosition);
            if (ball != null && (isFree || leftCurrentCoin >= ball.coin) && Input.GetTouch(0).deltaPosition == Vector2.zero)
            {//放置小球
                if (Input.GetTouch(0).phase == TouchPhase.Ended && !EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                {
                    //Debug.Log("开始调试"+Input.GetTouch(0).position);
                    Vector3 touchPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position) + 10 * Vector3.forward;
                    if (touchPos.x < 0)
                    {
                        GameObject newBall = Instantiate(ball.ball, touchPos, Quaternion.identity);
                        newBall.transform.parent = ballListGameObject.transform;  //将新生成的小球挂载到 BallList 物体上
                        BallList.instance.ballGameObjectList.Add(newBall);                          //将新生成的小球加入容器中
                        newBall.GetComponent<BallAi>().ballBlackBoard.ballFaction = BallBlackBoard.Faction.Left; //给小球加上阵营
                        newBall.GetComponent<SpriteRenderer>().material=greenOutlineMat;

                        PlaceSoldierToLeft(ball.coin);
                    }
                    else if (isFree && touchPos.x>0)
                    {
                        GameObject newBall = Instantiate(ball.ball, touchPos, Quaternion.identity);

                        newBall.transform.parent = ballListGameObject.transform;  //将新生成的小球挂载到 BallList 物体上
                        BallList.instance.ballGameObjectList.Add(newBall);                          //将新生成的小球加入容器中
                        newBall.GetComponent<BallAi>().ballBlackBoard.ballFaction = BallBlackBoard.Faction.Right; //给小球加上阵营
                       // newBall.GetComponent<SpriteRenderer>().material = redOutlineMat;

                        PlaceSoldierToRight(ball.coin);
                    }
                }
            }
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
