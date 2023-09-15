using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

//让战斗开始的脚本
public class OtherButton : MonoBehaviour
{
    public static OtherButton instance;
    private List<GameObject> ballList;
    public bool isStart; //战斗是否开始
    public List<GameObject> closeUI;
    private int levelMoney; //关卡初始时给的金币
    private GameObject front;  //上面部分的UI
    private GameObject under;  //下面部分的UI
    private bool isHide = false;       //是否隐藏了UI
    private bool isMove = false;       //UI是否在移动
    public void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        ballList = BallList.instance.ballGameObjectList;
        levelMoney = PlaceSoliderScript.instance.leftCurrentCoin;

        front = GameObject.Find("Front");
        under = GameObject.Find("Under");
        if (BallList.instance.sceneType!=BallList.SceneType.level)
        {
            DisplayUI();
        }
    }
    public void FigthStart() //让战斗开始的函数
    {
        if (BallList.instance.leftBallNum != 0 && BallList.instance.rightBallNum != 0)
        {
            Debug.Log("战斗开始");
            //先移动UI
            Transform frontT = front.GetComponent<RectTransform>();
            front.GetComponent<RectTransform>().DOMove(new Vector3(frontT.position.x, frontT.position.y + 155 * front.gameObject.transform.parent.transform.localScale.x, frontT.position.z), 1f);
            Transform underT = under.GetComponent<RectTransform>();
            under.GetComponent<RectTransform>().DOMove(new Vector3(underT.position.x, underT.position.y - 260 * under.gameObject.transform.parent.transform.localScale.x, underT.position.z), 1f).OnComplete(() =>
            {
                //关闭UI
                foreach (GameObject gameObject in closeUI)
                {
                    gameObject.SetActive(false);
                }
                GameObject.Find("HideButton").SetActive(false);
            });
            //改变小球状态
            foreach (GameObject ball in ballList)
            {
                ball.GetComponent<BallAi>().fsm.SwitchState(StateType.Move);
            }
            GameObject.Find("Square").SetActive(false);  //关闭分界线
            isStart = true; //战斗开始
        }
        else
        {
            SignUI.instance.DisplayText("未放置士兵，无法开始战斗", 1.5f, Color.white);
        }
        
    }
    public void ClearLeftBall() //清除左侧小球
    {
        if (BallList.instance.sceneType == BallList.SceneType.level)
        {
            PlaceSoliderScript.instance.leftCurrentCoin = levelMoney;
        }
        else if (BallList.instance.sceneType == BallList.SceneType.Free)
        {
            PlaceSoliderScript.instance.leftCurrentCoin = 0;
        }
        List<GameObject> ClearBall = new List<GameObject>();
        foreach (GameObject ball in BallList.instance.ballGameObjectList)
        {
            if (BallList.instance.ballBlackBoards[ball].ballFaction == BallBlackBoard.Faction.Left)
            {
                ClearBall.Add(ball);
            }
        }
        foreach (GameObject ball in ClearBall)
        {
            PlaceSoliderScript.instance.leftNumberOfSoldiers--;
            ballList.Remove(ball);
            Destroy(ball);
        }
        ClearBall.Clear();
        PlaceSoliderScript.instance.RefreshText();  //刷新金钱
    }
    public void ClearRightBall() //清除右侧小球
    {
        if (BallList.instance.sceneType == BallList.SceneType.level)
        {
            PlaceSoliderScript.instance.rightCurrentCoin = levelMoney;
        }
        else if (BallList.instance.sceneType == BallList.SceneType.Free)
        {
            PlaceSoliderScript.instance.rightCurrentCoin = 0;
        }
        List<GameObject> ClearBall = new List<GameObject>();
        foreach (GameObject ball in BallList.instance.ballGameObjectList)
        {
            if (BallList.instance.ballBlackBoards[ball].ballFaction == BallBlackBoard.Faction.Right)
            {
                ClearBall.Add(ball);
            }
        }
        foreach (GameObject ball in ClearBall)
        {
            PlaceSoliderScript.instance.rightNumberOfSoldiers--;
            ballList.Remove(ball);
            Destroy(ball);
        }
        ClearBall.Clear();
        PlaceSoliderScript.instance.RefreshText();  //刷新金钱
    }
    public void HideUI()
    {
        Transform hideButton = GameObject.Find("HideButton").transform;
        if (isHide == false && isMove == false)
        {
            isMove = true;
            Transform frontT = front.GetComponent<RectTransform>();
            front.GetComponent<RectTransform>().DOMove(new Vector3(frontT.position.x, frontT.position.y - 155 * front.gameObject.transform.parent.transform.localScale.x, frontT.position.z), 0.5f);
            Transform underT = under.GetComponent<RectTransform>();
            under.GetComponent<RectTransform>().DOMove(new Vector3(underT.position.x, underT.position.y + 260 * under.gameObject.transform.parent.transform.localScale.x, underT.position.z), 0.5f).OnComplete(() =>
            {
                isMove = false;
                hideButton.localScale = new Vector3(hideButton.localScale.x, -hideButton.localScale.y, hideButton.localScale.z);
            });
            isHide = true;
        }
        else if (isHide == true && isMove == false)
        {
            isMove = true;
            Transform frontT = front.GetComponent<RectTransform>();
            front.GetComponent<RectTransform>().DOMove(new Vector3(frontT.position.x, frontT.position.y + 155 * front.gameObject.transform.parent.transform.localScale.x, frontT.position.z), 0.5f);
            Transform underT = under.GetComponent<RectTransform>();
            under.GetComponent<RectTransform>().DOMove(new Vector3(underT.position.x, underT.position.y - 260 * under.gameObject.transform.parent.transform.localScale.x, underT.position.z), 0.5f).OnComplete(() =>
            {
                isMove = false;
                hideButton.localScale = new Vector3(hideButton.localScale.x, -hideButton.localScale.y, hideButton.localScale.z);
            });
            isHide = false;
        }
    }
    public void DisplayUI() //剧情对话结束后显示UI
    {
            isMove = true;
            Transform frontT = front.GetComponent<RectTransform>();
            front.GetComponent<RectTransform>().DOMove(new Vector3(frontT.position.x, frontT.position.y - 155 * front.gameObject.transform.parent.transform.localScale.x, frontT.position.z), 1f);
            Transform underT = under.GetComponent<RectTransform>();
            under.GetComponent<RectTransform>().DOMove(new Vector3(underT.position.x, underT.position.y + 260 * under.gameObject.transform.parent.transform.localScale.x, underT.position.z), 1f).OnComplete(() =>
            {
                isMove = false;
            });
            isHide = true;
    }
}
