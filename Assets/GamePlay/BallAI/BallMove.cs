using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class BallMove : MonoBehaviour
{
    private Rigidbody2D ballRigidbody;

    private BallInformation ballInformation;
    private void Awake()
    {
        ballRigidbody = GetComponent<Rigidbody2D>();  //绑定刚体组件
        ballInformation = new BallInformation(BallInformation.Faction.Attack, 1.5f, 0.4f, Vector2.right);
    }

    public void FixedUpdate()
    {
        AddForce(ballInformation.acceleration, ballInformation.initialOrientation);
        AddTorque(ballInformation.addAngularVelocity);
    }

    public void AddForce(float force, Vector2 initialOrientation)  //给球体施加其速度方向上的力
    {
        if (ballRigidbody.velocity != Vector2.zero)
        {
            float velocityAngle = Vector2.SignedAngle(Vector2.right, ballRigidbody.velocity);
            velocityAngle *= Mathf.Deg2Rad;
            float forceX = force * Mathf.Cos(velocityAngle);
            float forceY = force * Mathf.Sin(velocityAngle);
            ballRigidbody.AddForce(new Vector2(forceX, forceY));
            //Debug.Log(ballRigidbody.velocity);
        }
        else   //给球体初始速度
        {
            ballRigidbody.AddForce(initialOrientation);
        }
    }

    public void AddTorque(float torque)  //给球体增加角速度
    {
        float maxaAngularVelocity = 300f;  //物体的最大角速度
        if (ballRigidbody.angularVelocity < maxaAngularVelocity)
        {
            ballRigidbody.AddTorque(torque);
        }
    }

    private void AddInitialSpeed(Vector2 initialSpeed)       //给球体增加初始速度
    {
        ballRigidbody.AddForce(initialSpeed);
    }

}
