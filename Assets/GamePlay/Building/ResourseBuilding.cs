using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

/// <summary>
/// 资源建筑
/// </summary>
public abstract class ResourseBuilding : BuildingParent
{
    //生产的资源类型
    public ResourseManger.ResourseType resProduceType;

    /// <summary>
    /// 多少/每秒
    /// </summary>
    public int resourseMakeSpeed;

    //资源生成计时器
    private float resourseMakeTimer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MakeResourse();
    }

    //生成资源
    public virtual void MakeResourse()
    {
        resourseMakeTimer += Time.deltaTime;
        if (resourseMakeTimer > 1)
        {
            ResourseManger.Instance.ChangeResourse(resProduceType, resourseMakeSpeed);
            resourseMakeTimer = 0;
        }
    }
}
