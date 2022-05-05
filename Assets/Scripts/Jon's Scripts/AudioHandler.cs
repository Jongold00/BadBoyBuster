using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioHandler : MonoBehaviour
{
    static AudioHandler instance;

    public float musicVolume = 0.5f;
    public float sfxVolume = 0.5f;

    public AudioSource musicAudioSource;
    public AudioSource sfxAudioSource;


    [SerializeField]
    AudioClip musicClip;

    public static AudioHandler GetInstance()
    {
        if (!instance)
        {
            instance = GameObject.FindObjectOfType<AudioHandler>();
        }
        return instance;
    }

    public void Awake()
    {
        DontDestroyOnLoad(gameObject);
        musicAudioSource.clip = musicClip;
        musicAudioSource.loop = true;
        musicAudioSource.Play();
    }



    public void setMusicVolume(float set)
    {
        musicVolume = set;
        musicAudioSource.volume = musicVolume;
    }

    public void setSFXVolume(float set)
    {
        sfxVolume = set;
        sfxAudioSource.volume = sfxVolume;
    }

    public void PlaySoundEffect(AudioClip effect)
    {

    }
}
