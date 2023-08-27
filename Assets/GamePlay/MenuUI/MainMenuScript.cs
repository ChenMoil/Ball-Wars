using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField]
    GameObject settingsPanel; 
    [SerializeField]
    GameObject campaignPanel;
   
    public void ShowSettingsPanel(){
        settingsPanel.SetActive(true);
        ShowScrolls(settingsPanel.transform.Find("MiddleInformation").GetComponent<RectTransform>());
    }
     public void ShowCampaignPanel(){
        campaignPanel.SetActive(true);
        ShowScrolls(campaignPanel.transform.Find("MiddleInformation").GetComponent<RectTransform>());
    }

    public void ShowScrolls(RectTransform rectTransform){   //¾íÖá¹ö¶¯Ð§¹û
        rectTransform.sizeDelta = new Vector2(0, 1008);
        rectTransform.DOSizeDelta(new Vector2(1704, 1008),1f);
    }
}
