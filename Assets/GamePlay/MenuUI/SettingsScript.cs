using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsScript : MonoBehaviour
{
    [SerializeField] Slider musicVolumeSlider;  //音乐音量滑块
    [SerializeField] Slider soundEffectVolumeSlider; //音效音量滑块
    bool isHide=true;  //设置面板移动动画
    bool isMove=false;
    GameObject informationPanel;//设置面板
    private void Start()
    {
        musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1f);
        soundEffectVolumeSlider.value = PlayerPrefs.GetFloat("SoundEffectVolume", 1f);
        informationPanel = transform.Find("MiddleInformation").gameObject;
    }
    public void ChangeMusicVolume(float volume)        //改变音量
    {
        PlayerPrefs.SetFloat("MusicVolume", volume);
        GameObject.Find("AudioManager").GetComponent<AudioManagerScript>().UpdateVolume();
    }
    public void ChangeSoundEffectVolume(float volume)
    {
        PlayerPrefs.SetFloat("SoundEffectVolume",volume);
        GameObject.Find("AudioManager").GetComponent<AudioManagerScript>().UpdateVolume();
    }
    public void ChangePanelStatus()      //改变设置面板的显示状态
    {
        if (!isMove)
        {
            isMove = true;
            if (isHide)
            {
                informationPanel.GetComponent<RectTransform>().DOMove(new Vector3(informationPanel.transform.position.x, informationPanel.transform.position.y - 718 * transform.localScale.y, informationPanel.transform.position.z), 0.75f).OnComplete(() =>
                {
                    isMove = false;
                    isHide = false;
                });
            }
            else
            {
                informationPanel.GetComponent<RectTransform>().DOMove(new Vector3(informationPanel.transform.position.x, informationPanel.transform.position.y + 718 * transform.localScale.y, informationPanel.transform.position.z), 0.75f).OnComplete(() =>
                {
                    isMove = false;
                    isHide = true;
                });
            }
        }
    }
    public void RetuenMainMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
