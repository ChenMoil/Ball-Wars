using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourseManger : MonoBehaviour
{
    public ResourseManger()
    {
        
        InitResourse();
    } 

    private static ResourseManger instance;
    public static ResourseManger Instance {  
        get 
        { 
            if (instance != null)
            {
                return instance;
            }
            else { Debug.LogError("未找到ResourseManger单例"); return null; }
        } 
    }
    public enum ResourseType
    {
        food
    }

    //资源数量
    public Dictionary<ResourseType, int> resourseNumber;

    public void InitResourse()
    {
        resourseNumber = new Dictionary<ResourseType, int>();

        //每个资源都需要进行初始化
        resourseNumber[ResourseType.food] = 0;
    }

    /// <summary>
    /// 改变资源数量
    /// </summary>
    /// <param name="type">资源类型</param>
    /// <param name="number">数量</param>
    public void ChangeResourse(ResourseType type, int number)
    {
        resourseNumber[type] += number;
        Debug.Log(type + resourseNumber[type]);
    }

    private void Awake()
    {
        instance = this;
    }
}
