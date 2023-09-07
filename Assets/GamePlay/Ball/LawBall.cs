using System;
using System.Collections;
using UnityEngine;

public class LawBall: MonoBehaviour  //法球
{
    [NonSerialized] public int Damage;  //武器攻击力
    public BallBlackBoard.Faction lawBallFation;
    private GameObject Explode;          //爆炸效果
    private void Start()
    {
        Explode = transform.Find("Explode").gameObject;
        StartCoroutine(DestoryLawBall(5f));
    }
    private void OnCollisionEnter2D(Collision2D collision)  //法球与其他物体发生碰撞
    {
        if (collision.gameObject.tag == "BallSoldier" && lawBallFation != collision.gameObject.GetComponent<BallAi>().ballBlackBoard.ballFaction)
        {
            Explode.GetComponent<LawBallExplode>().StartCon();
            StopAllCoroutines();
            StartCoroutine(DestoryLawBall(1f));
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            //collision.gameObject.GetComponent<BallAi>().DeductHP(Damage);  //扣血
            //Destroy(gameObject);
        }
        else if(collision.gameObject.tag == "Boundary")
        {
            Destroy(gameObject);
        }
    }
    IEnumerator DestoryLawBall(float time) //定时销毁法球
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
