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
    public void ShowLevelInfo(int index)
    {
        GameObject level = Instantiate(levelInfoPrefab, campaignPanel.transform);
        level.transform.Find("LevelName").GetComponent<Text>().text = levelInfos[index].levelName;
        level.transform.Find("LevelImage").GetComponent<Image>().sprite = levelInfos[index].levelImage;
        level.transform.Find("LevelDescription").GetComponent<Text>().text = levelInfos[index].levelDescription;
        level.transform.Find("LevelButton").GetComponent<Button>().onClick.AddListener(()=> LoadLevel(levelInfos[index].levelSceneIndex));
    }
    public void LoadLevel(int index)
    {
        SceneManager.LoadScene(index);
    }
}
