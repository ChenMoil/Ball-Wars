using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public class levelInfo          //�ؿ���Ϣ��
{
    public string levelName;       //�ؿ�����
    public Sprite levelImage;   //�ؿ�ͼƬ
    public string levelDescription;//�ؿ�����
    public int levelSceneIndex;    //�ؿ������±�
}
public class LevelManagerScript : MonoBehaviour
{
    [SerializeField] List<levelInfo> levelInfos;
    [SerializeField] GameObject levelInfoPrefab;
    [SerializeField] GameObject campaignPanel;
    [SerializeField] List<Button> buttons;   //ѡ��ؿ��İ�ť
    private void Start()
    {
        UpdateLevel(PlayerPrefs.GetInt("Level", 0));
    }
    public void ShowLevelInfo(int index)        //��ʾ�ؿ���Ϣ
    {
        GameObject level = Instantiate(levelInfoPrefab, campaignPanel.transform);
        level.transform.Find("LevelName").GetComponent<Text>().text = levelInfos[index].levelName;   ///��ʼ��
        level.transform.Find("LevelImage").GetComponent<Image>().sprite = levelInfos[index].levelImage;
        level.transform.Find("LevelDescription").GetComponent<Text>().text = levelInfos[index].levelDescription;
        level.transform.Find("LevelButton").GetComponent<Button>().onClick.AddListener(()=> LoadLevel(levelInfos[index].levelSceneIndex));
        level.transform.Find("ReturnButton").GetComponent<Button>().onClick.AddListener(() => ReturnMenu());
        level.name = "LevelInfo";
    }
    public void LoadLevel(int index)   //���عؿ�����
    {
        SceneManager.LoadScene(index);
    }
    public void ReturnMenu()    //�رչؿ���Ϣ����
    {
        Destroy(campaignPanel.transform.Find("LevelInfo").gameObject);
    }
    void UpdateLevel(int lockedNum)   //���¹ؿ��������
    {
        for (int i = 0; i <= lockedNum; i++)
        {
            buttons[i].interactable = true;
        }
    }
}
