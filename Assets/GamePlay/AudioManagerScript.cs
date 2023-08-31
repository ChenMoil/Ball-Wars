using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagerScript : MonoBehaviour
{
    [SerializeField] AudioClip musicClip;   //默认播放背景音乐
    AudioSource audioSource;                //音源
    float musicVolume;                //音乐音量
    float soundEffectVolume;          //音效音量
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        UpdateVolume();
        if(musicClip != null)
            PlayMusic(musicClip);
    }
    public void UpdateVolume()   //更新音量
    {
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);     
        soundEffectVolume = PlayerPrefs.GetFloat("SoundEffectVolume", 1f);
        audioSource.volume = musicVolume;
    }
    public void PlayMusic(AudioClip clip )
    {
        audioSource.Stop();
        audioSource.clip = clip;
        audioSource.volume = musicVolume;
        audioSource.Play();
    }
    public void PlaySoundEffect(AudioClip clip)
    {
        audioSource.PlayOneShot(clip,soundEffectVolume);
    }
}
