using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsScript : MonoBehaviour
{
    [SerializeField] Slider musicVolumeSlider;  //音乐音量滑块
    [SerializeField] Slider soundEffectVolumeSlider; //音效音量滑块
    bool isHide;  //设置面板移动动画
    bool isMove;
    private void Start()
    {
        musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1f);
        soundEffectVolumeSlider.value = PlayerPrefs.GetFloat("SoundEffectVolume", 1f);
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
