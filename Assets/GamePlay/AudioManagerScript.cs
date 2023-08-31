using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagerScript : MonoBehaviour
{
    [SerializeField] AudioClip musicClip;   //Ĭ�ϲ��ű�������
    AudioSource audioSource;                //��Դ
    float musicVolume;                //��������
    float soundEffectVolume;          //��Ч����
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        UpdateVolume();
        if(musicClip != null)
            PlayMusic(musicClip);
    }
    public void UpdateVolume()   //��������
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
