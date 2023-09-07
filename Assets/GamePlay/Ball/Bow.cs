using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Bow : MonoBehaviour
{
    private GameObject arrowPic;

    public GameObject arrow;   //射出去的箭的预制体

    public int arrowDamage;   //射出去的箭的伤害

    public float arrowForce;   //射出去的施加的力

    private Transform ballTransform;  //小球的位置信息
    // Start is called before the first frame update
    void Start()
    {
        ballTransform = gameObject.transform.parent;
        if (gameObject.transform.childCount != 0)
        {
            arrowPic = gameObject.transform.GetChild(0).gameObject;
        }
    }

    public void OpenArrowPic() 
    {
        if (arrowPic != null)
        {
            arrowPic.SetActive(true);
        }
    }
    public void CloseArrowPic()
    {
        if (arrowPic != null)
        {
            arrowPic.SetActive(false);
        }
    }

    public void Archery()   //射箭
    {
        GameObject newArrow = Instantiate(arrow, gameObject.transform.position, ballTransform.rotation);
        Rigidbody2D arrowrRigidbody2D = newArrow.GetComponent<Rigidbody2D>();
        float eur = gameObject.transform.eulerAngles.z * Mathf.Deg2Rad;
        float forceX = (float)Math.Cos(eur);
        float forceY = (float)Math.Sin(eur);
        newArrow.transform.position = new Vector2(gameObject.transform.position.x + 1 * forceX, gameObject.transform.position.y + 1 * forceY); //调整位置
        arrowrRigidbody2D.AddForce(new Vector2(forceX * arrowForce, forceY * arrowForce), ForceMode2D.Impulse); //施加力
        if (newArrow.GetComponent<Arrow>() != null)
        {
            newArrow.GetComponent<Arrow>().arrowFation = BallList.instance.ballBlackBoards[gameObject.transform.parent.gameObject].ballFaction;   //改变阵营
            newArrow.GetComponent<Arrow>().Damage = arrowDamage;
        }
        else if (newArrow.GetComponent<LawBall>() != null)
        {
            newArrow.GetComponent<LawBall>().lawBallFation = BallList.instance.ballBlackBoards[gameObject.transform.parent.gameObject].ballFaction;   //改变阵营
            newArrow.GetComponent<LawBall>().Damage = arrowDamage;
        }
        newArrow.transform.parent = GameObject.Find("FlyingObjects").transform;

    }
}
