using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameEnd : MonoBehaviour
{
    static public void CheckGameEnd(BallBlackBoard.Faction faction)  //检查该场游戏是否结束
    {
        if (faction == BallBlackBoard.Faction.Left)  //根据阵营减少数量
        {
            BallList.instance.leftBallNum--;
        }
        else
        {
            BallList.instance.rightBallNum--;
        }
        if (BallList.instance.rightBallNum <= 0)
        {
            GameEnd gameEnd = GameObject.Find("GameEnd").GetComponent<GameEnd>();
            gameEnd.StartCoroutine(ShowEndUI("win"));
        }
        else if (BallList.instance.leftBallNum <= 0)
        {
            GameEnd gameEnd = GameObject.Find("GameEnd").GetComponent<GameEnd>();
            gameEnd.StartCoroutine(ShowEndUI("lose"));
        }
    }
    public void ReturnMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    static IEnumerator ShowEndUI(string str)
    {
        if (BallList.instance.sceneType != BallList.SceneType.Test)
        {
            if (str == "win")
            {
                PlayPlotManagerScript playPlot = GameObject.Find("GamePlot").GetComponent<PlayPlotManagerScript>();
                playPlot.PostPlayDialog();
                yield return new WaitUntil(() => !playPlot.IsPostPlay);
            }
            string imagePath = "Image/" + str;
            Sprite image=Resources.Load<Sprite>(imagePath);
            GameObject endUI = GameObject.Find("GameEnd");
            if (endUI == null)
            {
                Debug.LogError("未找到endUI");
            }
            for (int i= 0;i<endUI.transform.childCount;i++)
            {
                endUI.transform.GetChild(i).gameObject.SetActive(true);
            }
            if (endUI.transform.Find("EndImage").gameObject.GetComponent<Image>() == null)
            {
                Debug.LogError("未找到endUI的Image");
            }
            endUI.transform.Find("EndImage").gameObject.GetComponent<Image>().sprite = image;
        }
    }
}
