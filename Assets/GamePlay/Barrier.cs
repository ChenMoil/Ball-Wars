using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour
{
    public int HP;  //障碍物的血量
    // Start is called before the first frame update
    public void DeductHP(int attack) //扣血函数
    {
        HP -= attack;
        if (HP <=0 )
        {
            Destroy(gameObject);
        }
    }

}
