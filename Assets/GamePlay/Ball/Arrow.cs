using System;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [NonSerialized] public int Damage;  //武器攻击力
    public BallBlackBoard.Faction arrowFation;
    private void Start()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)  //武器与其他物体发生碰撞
    {
        if (collision.gameObject.tag == "BallSoldier" && arrowFation != collision.gameObject.GetComponent<BallAi>().ballBlackBoard.ballFaction)
        {
            collision.gameObject.GetComponent<BallAi>().DeductHP(Damage);  //扣血
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
