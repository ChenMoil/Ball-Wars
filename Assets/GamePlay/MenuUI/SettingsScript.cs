using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsScript : MonoBehaviour
{
    [SerializeField] Slider musicVolumeSlider;  //“Ù¿÷“Ù¡øª¨øÈ
    [SerializeField] Slider soundEffectVolumeSlider; //“Ù–ß“Ù¡øª¨øÈ
    private void Start()
    {
        musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1f);
        soundEffectVolumeSlider.value = PlayerPrefs.GetFloat("SoundEffectVolume", 1f);
    }
    public void ChangeMusicVolume(float volume)
    {
        PlayerPrefs.SetFloat("MusicVolume", volume);
        GameObject.Find("AudioManager").GetComponent<AudioManagerScript>().UpdateVolume();
    }
    public void ChangeSoundEffectVolume(float volume)
    {
        PlayerPrefs.SetFloat("SoundEffectVolume",volume);
        GameObject.Find("AudioManager").GetComponent<AudioManagerScript>().UpdateVolume();
    }
}
