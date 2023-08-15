using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//近战武器攻击的判断脚本
public class MeleeWeapon : MonoBehaviour
{
    [SerializeField] private int Attack;  //武器攻击力
    private void OnCollisionEnter2D(Collision2D collision)  //武器与其他物体发生碰撞
    {
        if (collision.gameObject.tag == "BallSoldier" && gameObject.GetComponentInParent<BallAi>().ballBlackBoard.ballFaction != collision.gameObject.GetComponent<BallAi>().ballBlackBoard.ballFaction)
        {
            collision.gameObject.GetComponent<BallAi>().DeductHP(Attack);  //扣血
            Debug.Log(gameObject.name + "对" + collision.gameObject.name + "造成了" + Attack + " 点伤害");
        }
    }
}
