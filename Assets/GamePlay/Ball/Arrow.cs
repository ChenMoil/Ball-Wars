using System;
using System.Collections;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [NonSerialized] public int Damage;  //武器攻击力
    public BallBlackBoard.Faction arrowFation;
    private void Start()
    {
        StartCoroutine(DestoryArrow());
    }
    private void OnCollisionEnter2D(Collision2D collision)  //武器与其他物体发生碰撞
    {
        if (collision.gameObject.tag == "BallSoldier" && arrowFation != collision.gameObject.GetComponent<BallAi>().ballBlackBoard.ballFaction)
        {
            collision.gameObject.GetComponent<BallAi>().DeductHP(Damage);  //扣血
            Destroy(gameObject);
        }
        else if(collision.gameObject.tag == "Boundary")
        {
            Destroy(gameObject);
        }
    }
    IEnumerator DestoryArrow() //定时销毁箭
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }
}
