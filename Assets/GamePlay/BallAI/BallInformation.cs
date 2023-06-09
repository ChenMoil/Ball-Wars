using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallInformation
{
    public enum Faction   //判断为进攻方还是防守方
    {
        Attack, Defend
    }
    public Faction ballFaction;     //球体的阵营

    public float acceleration;   //球体移动的加速度

    public float addAngularVelocity;   //球体移动的加角速度

    public Vector2 initialOrientation;  //球体移动的初始方向

    public BallInformation(Faction ballFaction, float acceleration, float addAngularVelocity, Vector2 initialOrientation)
    {
        this.ballFaction = ballFaction;
        this.acceleration = acceleration;
        this.addAngularVelocity = addAngularVelocity;
        this.initialOrientation = initialOrientation;
    }
}
