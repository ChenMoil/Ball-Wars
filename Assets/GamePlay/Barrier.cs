using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour
{
    public int HP;  //障碍物的血量

    public BallBlackBoard.Faction faction;  //障碍物的阵营
    public void DeductHP(int attack) //扣血函数
    {
        HP -= attack;
        if (HP <=0 )
        {
            Destroy(gameObject);
        }
    }

}
