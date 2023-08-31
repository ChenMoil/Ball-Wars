using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public class levelInfo          //关卡信息类
{
    public string levelName;       //关卡名字
    public Sprite levelImage;   //关卡图片
    public string levelDescription;//关卡描述
    public int levelSceneIndex;    //关卡场景下标
}
public class LevelManagerScript : MonoBehaviour
{
    [SerializeField] List<levelInfo> levelInfos;
    [SerializeField] GameObject levelInfoPrefab;
    [SerializeField] GameObject campaignPanel;
    [SerializeField] List<Button> buttons;   //选择关卡的按钮
    private void Start()
    {
        UpdateLevel(PlayerPrefs.GetInt("Level", 0));
    }
    public void ShowLevelInfo(int index)        //显示关卡信息
    {
        GameObject level = Instantiate(levelInfoPrefab, campaignPanel.transform);
        level.transform.Find("LevelName").GetComponent<Text>().text = levelInfos[index].levelName;   ///初始化
        level.transform.Find("LevelImage").GetComponent<Image>().sprite = levelInfos[index].levelImage;
        level.transform.Find("LevelDescription").GetComponent<Text>().text = levelInfos[index].levelDescription;
        level.transform.Find("LevelButton").GetComponent<Button>().onClick.AddListener(()=> LoadLevel(levelInfos[index].levelSceneIndex));
        level.transform.Find("ReturnButton").GetComponent<Button>().onClick.AddListener(() => ReturnMenu());
        level.name = "LevelInfo";
    }
    public void LoadLevel(int index)   //加载关卡场景
    {
        SceneManager.LoadScene(index);
    }
    public void ReturnMenu()    //关闭关卡信息界面
    {
        Destroy(campaignPanel.transform.Find("LevelInfo").gameObject);
    }
    void UpdateLevel(int lockedNum)   //更新关卡解锁情况
    {
        for (int i = 0; i <= lockedNum; i++)
        {
            buttons[i].interactable = true;
        }
    }
}
