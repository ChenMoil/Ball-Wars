using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameEnd : MonoBehaviour
{
    static public void CheckGameEnd(BallBlackBoard.Faction faction)  //���ó���Ϸ�Ƿ����
    {
        if (faction == BallBlackBoard.Faction.Left)
        {
            BallList.instance.leftBallNum--;
        }
        else
        {
            BallList.instance.rightBallNum--;
        } 
        if (BallList.instance.rightBallNum <= 0)
        {
            ShowEndUI("�ɹ�");
        }
        else if (BallList.instance.leftBallNum<=0)
        {
            ShowEndUI("ʧ��");
        }
    }
    public void ReturnMenu()
    {
        SceneManager.LoadSceneAsync(0);
    }
    static void ShowEndUI(string str)
    {
        GameObject endUI = GameObject.Find("GameEnd");
        if (endUI == null)
        {
            Debug.LogError("δ�ҵ�endUI");
        }
        endUI.SetActive(true);
        endUI.transform.Find("Text").GetComponent<Text>().text = str;
    }
}
