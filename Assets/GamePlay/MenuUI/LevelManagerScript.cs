using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelConst
{
    static Dictionary<string, int> m_levelIndex = new Dictionary<string, int>()
    {
        ["Level1"] = 1,
        ["Level2"] = 2,
        ["Level3"] = 3,
        ["Level4"] = 4,
        ["Level5"] = 5
    };
    public static Dictionary<string, int> LevelIndex
    {
        get { return m_levelIndex; }
    }
}

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
    [SerializeField] Sprite passedLevel;   //通过关卡的图标
    bool isShow;  //当前是否存在关卡信息界面
    private void Start()
    {
        UpdateLevel(PlayerPrefs.GetInt("Level", 0));
    }
    public void ShowLevelInfo(int index)        //显示关卡信息
    {
        if (isShow)
        {
            return;
        }
        isShow = true;
        GameObject level = Instantiate(levelInfoPrefab, campaignPanel.transform);
        level.transform.Find("LevelName").GetComponent<Text>().text = levelInfos[index].levelName;   ///初始化
        level.transform.Find("LevelImage").GetComponent<Image>().sprite = levelInfos[index].levelImage;
        level.transform.Find("LevelDescription").GetComponent<Text>().text = levelInfos[index].levelDescription;
        level.transform.Find("LevelButton").GetComponent<Button>().onClick.AddListener(()=> LoadLevel(levelInfos[index].levelSceneIndex));
        level.transform.Find("ReturnButton").GetComponent<Button>().onClick.AddListener(() => ReturnMenu());
        level.name = "LevelInfo";

        level.GetComponent<CanvasGroup>().DOFade(1, 1.5f);
        level.GetComponent<RectTransform>().anchoredPosition = new Vector2(campaignPanel.GetComponent<RectTransform>().rect.width, 0);
        level.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, 0), 1f);
        level.GetComponent<RectTransform>().sizeDelta=campaignPanel.GetComponent<RectTransform>().sizeDelta;
    }
    public void LoadLevel(int index)   //加载关卡场景
    {
        SceneManager.LoadScene(index);
    }
    public void ReturnMenu()    //关闭关卡信息界面
    {
        GameObject level = GameObject.Find("LevelInfo");
        level.GetComponent<CanvasGroup>().DOFade(0, 1f);
        level.GetComponent<RectTransform>().DOAnchorPos(new Vector2(campaignPanel.GetComponent<RectTransform>().rect.width/2, 0), 1f).OnComplete(() =>
        {
            Destroy(campaignPanel.transform.Find("LevelInfo").gameObject);
            isShow = false;
        });     
    }
    void UpdateLevel(int lockedNum)   //更新关卡解锁情况
    {
        for (int i = 0; i <= lockedNum; i++)
        {
            if(buttons.Count>i)
            buttons[i].interactable = true;
        }
        for (int i = 0; i < lockedNum; i++)
        {
            buttons[i].gameObject.GetComponent<Image>().sprite = passedLevel;
        }
    }
}
