using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsScript : MonoBehaviour
{
    [SerializeField] Slider musicVolumeSlider;  //������������
    [SerializeField] Slider soundEffectVolumeSlider; //��Ч��������
    bool isHide;  //��������ƶ�����
    bool isMove;
    private void Start()
    {
        musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1f);
        soundEffectVolumeSlider.value = PlayerPrefs.GetFloat("SoundEffectVolume", 1f);
    }
    public void ChangeMusicVolume(float volume)        //�ı�����
    {
        PlayerPrefs.SetFloat("MusicVolume", volume);
        GameObject.Find("AudioManager").GetComponent<AudioManagerScript>().UpdateVolume();
    }
    public void ChangeSoundEffectVolume(float volume)
    {
        PlayerPrefs.SetFloat("SoundEffectVolume",volume);
        GameObject.Find("AudioManager").GetComponent<AudioManagerScript>().UpdateVolume();
    }
    public void ChangePanelStatus()      //�ı�����������ʾ״̬
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
