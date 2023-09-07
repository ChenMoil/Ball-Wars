using DG.Tweening;
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
    [SerializeField] Sprite passedLevel;   //ͨ���ؿ���ͼ��
    bool isShow;  //��ǰ�Ƿ���ڹؿ���Ϣ����
    private void Start()
    {
        UpdateLevel(PlayerPrefs.GetInt("Level", 0));
    }
    public void ShowLevelInfo(int index)        //��ʾ�ؿ���Ϣ
    {
        if (isShow)
        {
            return;
        }
        isShow = true;
        GameObject level = Instantiate(levelInfoPrefab, campaignPanel.transform);
        level.transform.Find("LevelName").GetComponent<Text>().text = levelInfos[index].levelName;   ///��ʼ��
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
    public void LoadLevel(int index)   //���عؿ�����
    {
        SceneManager.LoadScene(index);
    }
    public void ReturnMenu()    //�رչؿ���Ϣ����
    {
        GameObject level = GameObject.Find("LevelInfo");
        level.GetComponent<CanvasGroup>().DOFade(0, 1f);
        level.GetComponent<RectTransform>().DOAnchorPos(new Vector2(campaignPanel.GetComponent<RectTransform>().rect.width/2, 0), 1f).OnComplete(() =>
        {
            Destroy(campaignPanel.transform.Find("LevelInfo").gameObject);
            isShow = false;
        });     
    }
    void UpdateLevel(int lockedNum)   //���¹ؿ��������
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
