using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaceSoliderScript : MonoBehaviour
{
    [SerializeField] int currentCoin;
    BallBlackBoard ball;
    [SerializeField] Text textCoin;
    void Awake()
    {
        textCoin.text=currentCoin.ToString();
    }
    void Update(){
        if(ball != null && currentCoin >= ball.coin){
            if (Input.GetTouch(0).phase==TouchPhase.Began)
            {
                Instantiate(ball.ball, Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position), Quaternion.identity);
                ChangeCoin(ball.coin);
            }
        }
    }
    public void ChangeBall(BallBlackBoard newBall){
        ball = newBall;
    }
    public void ChangeBall()
    {
        ball = null;
    }
    void ChangeCoin(int amount)
    {
        currentCoin+=amount;
        textCoin.text = currentCoin.ToString();
    }
}
