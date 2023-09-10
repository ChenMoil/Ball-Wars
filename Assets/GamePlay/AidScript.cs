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
    int loseCount; //��ͬһ������ʧ�ܵĴ���
    int currentLostLevel; //��ǰʧ�ܵĹؿ�
    int aidCount=2;  //��Ԯ����ʱ��ʧ�ܴ���
    bool isAid;  //�Ƿ���Ԯ
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
                new Dialogues("���࣬����������������!",Dialogues.characterDirection.Left,"����",dwarf)
            };
        }
    PlaceSoliderScript place = GameObject.FindObjectOfType<PlaceSoliderScript>();
        place.ballList.Add(Resources.Load<SummonBall>("Prefabs/���˽�ʿ"));
    }
}
