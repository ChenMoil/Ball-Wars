using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BallInformation;

public class CreateManger : MonoBehaviour
{
    [SerializeField]private GameObject testBall;

    private void Start()
    {
        
    }

    public void CreateBall(GameObject ball, Faction ballFaction, float acceleration, float addAngularVelocity)
    {
        Instantiate(ball);
        
    }
}
