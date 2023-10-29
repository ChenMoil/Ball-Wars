using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
            if (BallList.instance.sceneType == BallList.SceneType.level)    //增援系统判断
            {
                AidScript.Instance.Win();
            }
        }
        else if (BallList.instance.leftBallNum <= 0)
        {
            GameEnd gameEnd = GameObject.Find("GameEnd").GetComponent<GameEnd>();
            gameEnd.StartCoroutine(ShowEndUI("lose"));
            if (BallList.instance.sceneType==BallList.SceneType.level)     //增援系统判断
            {
                AidScript.Instance.Lose(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }
    public void ReturnMenu()
    {
        SceneManager.LoadScene(0);
    }
    public static void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public static void NextLevel()
    {
        SceneManager.LoadScene(LevelConst.LevelIndex["Level"+ (int.Parse(SceneManager.GetActiveScene().name.Remove(0, 5))+1)]);
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
                if(SceneManager.GetActiveScene().name.Substring(0,5)=="Level")
                    PlayerPrefs.SetInt("Level",int.Parse(SceneManager.GetActiveScene().name.Remove(0,5)));
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

            GameObject reButton = endUI.transform.Find("RestartButton").gameObject;
            if (str=="win" && LevelConst.LevelIndex.ContainsKey("Level" + (int.Parse(SceneManager.GetActiveScene().name.Remove(0, 5)) + 1)))
            {
                reButton.GetComponent<Button>().onClick.AddListener(()=>NextLevel());
                reButton.GetComponent<Image>().sprite = Resources.Load<Sprite>("Image/next");
                reButton.GetComponent<Image>().SetNativeSize();
            }
            else
            {
                reButton.GetComponent<Button>().onClick.AddListener(() => RestartLevel());
                reButton.GetComponent<Image>().sprite = Resources.Load<Sprite>("Image/restart");
                reButton.GetComponent<Image>().SetNativeSize();
            }
        }
    }
}
