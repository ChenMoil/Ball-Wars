using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//让战斗开始的脚本
public class OtherButton : MonoBehaviour
{
    private List<GameObject> ballList;
    private PlaceSoliderScript place;  //放置士兵的脚本
    public List<GameObject> closeUI;
    private int levelMoney;
    private GameObject front;  //上面部分的UI
    private GameObject under;  //下面部分的UI
    private bool isHide = false;       //是否隐藏了UI
    private bool isMove = false;       //UI是否在移动
    public void Start()
    {
        ballList = BallList.instance.ballGameObjectList;
        levelMoney = GameObject.Find("Place soldier").GetComponent<PlaceSoliderScript>().leftCurrentCoin;
        place = GameObject.Find("Place soldier").GetComponent<PlaceSoliderScript>();
        front = GameObject.Find("Front");
        under = GameObject.Find("Scroll View");
    }
    public void FigthStart() //让战斗开始的函数
    {
        Debug.Log("战斗开始");
        //先移动UI
        Transform frontT = front.GetComponent<RectTransform>();
        front.GetComponent<RectTransform>().DOMove(new Vector3(frontT.position.x, frontT.position.y + 100, frontT.position.z), 1f);
        Transform underT = under.GetComponent<RectTransform>();
        under.GetComponent<RectTransform>().DOMove(new Vector3(underT.position.x, underT.position.y - 160, underT.position.z), 1f).OnComplete(() =>
        {
            //关闭UI
            foreach (GameObject gameObject in closeUI)
            {
                gameObject.SetActive(false);
            }
        });
        //改变小球状态
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
        GameObject.Find("Square").SetActive(false);  //关闭分界线
        place.isStart = true; //战斗开始
    }
    public void Clear()
    {
        GameObject.Find("Place soldier").GetComponent<PlaceSoliderScript>().leftCurrentCoin = levelMoney;
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
            ballList.Remove(ball);
            Destroy(ball);
        }
        ClearBall.Clear();
    }
    public void HideUI()
    {
        if (isHide == false && isMove == false)
        {
            isMove = true;
            Transform frontT = front.GetComponent<RectTransform>();
            front.GetComponent<RectTransform>().DOMove(new Vector3(frontT.position.x, frontT.position.y + 200, frontT.position.z), 1f);
            Transform underT = under.GetComponent<RectTransform>();
            under.GetComponent<RectTransform>().DOMove(new Vector3(underT.position.x, underT.position.y - 280, underT.position.z), 1f).OnComplete(() =>
            {
                isMove = false;
            });
            isHide = true;
        }
        else if (isHide == true && isMove == false)
        {
            isMove = true;
            Transform frontT = front.GetComponent<RectTransform>();
            front.GetComponent<RectTransform>().DOMove(new Vector3(frontT.position.x, frontT.position.y - 200, frontT.position.z), 1f);
            Transform underT = under.GetComponent<RectTransform>();
            under.GetComponent<RectTransform>().DOMove(new Vector3(underT.position.x, underT.position.y + 280, underT.position.z), 1f).OnComplete(() =>
            {
                isMove = false;
            });
            isHide = false;
        }
    }
}
