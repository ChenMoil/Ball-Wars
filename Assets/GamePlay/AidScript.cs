using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AidScript 
{
    static AidScript instance;
    public static AidScript Instance
    {
        get {
            if (instance == null)
                instance = new AidScript();
            return instance; 
        }
    }
    int loseCount; //在同一关连续失败的次数
    int currentLostLevel; //当前失败的关卡
    int aidCount=1;  //增援出现时的失败次数
    bool isAid;  //是否增援
    public void Lose(int index)
    {
        if (index==currentLostLevel)
        {
            loseCount++;
            if (loseCount>=aidCount)
            {
                isAid =true;
            }
        }
        else
        {
            loseCount = 0;
            loseCount++;
            currentLostLevel = index;  
        }
    }
    public void Win()
    {
        loseCount = 0;
        currentLostLevel = -1;
    }
    public void Aid()
    {
        if (!isAid)
        {
            return;
        }
        if (loseCount==aidCount)
        {
            PlayPlotManagerScript playPlot = GameObject.FindObjectOfType<PlayPlotManagerScript>();
            Sprite dwarf = Resources.Load<Sprite>("Image/dwarf");
            playPlot.prelines = new List<Dialogues>
            {
                new Dialogues("人类，矮人和猫猫虫来帮助你们了!",Dialogues.characterDirection.Left,"矮人",dwarf)
            };
        }
    PlaceSoliderScript place = GameObject.FindObjectOfType<PlaceSoliderScript>();
        place.ballList.Add(Resources.Load<SummonBall>("Prefabs/矮人剑士"));
        place.ballList.Add(Resources.Load<SummonBall>("Prefabs/猫猫枪手"));
    }
}
