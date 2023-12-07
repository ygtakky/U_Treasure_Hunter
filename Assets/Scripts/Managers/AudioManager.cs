using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [Header("Listening to")]
    [SerializeField] private AudioEventChannelSO musicEventChannel;
    [SerializeField] private AudioEventChannelSO sfxEventChannel;
    
    [Header("Configuration")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource[] sfxSources;
    [SerializeField] private AudioMixerGroup musicMixerGroup;
    [SerializeField] private AudioMixerGroup sfxMixerGroup;
    [SerializeField][Range(-80, 20)] private int sfxVolume;
    [SerializeField][Range(-80, 20)] private int musicVolume;
    
    private const int MAX_NUMBER_OF_SFX_SOURCES = 10;

    private void Awake()
    {
        musicSource.loop = true;
        
        if (sfxSources == null || sfxSources.Length < MAX_NUMBER_OF_SFX_SOURCES)
        {
            sfxSources = new AudioSource[MAX_NUMBER_OF_SFX_SOURCES];
            for (int i = 0; i < MAX_NUMBER_OF_SFX_SOURCES; i++)
            {
                sfxSources[i] = gameObject.AddComponent<AudioSource>();
            }
        }

        foreach (AudioSource audioSource in sfxSources)
        {
            audioSource.loop = false;
            audioSource.enabled = false;
        }
        
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        musicMixerGroup.audioMixer.SetFloat("MusicVolume", musicVolume);
        sfxMixerGroup.audioMixer.SetFloat("SFXVolume", sfxVolume);
    }

    private void OnEnable()
    {
        musicEventChannel.OnEventRaised += MusicEventChannel_OnEventRaised;
        sfxEventChannel.OnEventRaised += SFXEventChannel_OnEventRaised;
    }

    private void OnDisable()
    {
        musicEventChannel.OnEventRaised -= MusicEventChannel_OnEventRaised;
        sfxEventChannel.OnEventRaised -= SFXEventChannel_OnEventRaised;
    }
    
    private void MusicEventChannel_OnEventRaised(object sender, AudioEventArgs e)
    {
        PlayMusic(e.audioClip);
    }
    
    private void SFXEventChannel_OnEventRaised(object sender, AudioEventArgs e)
    {
        StartCoroutine(PlaySFX(e.audioClip));
    }

    private void PlayMusic(AudioClip musicClip)
    {
        musicSource.clip = musicClip;
        musicSource.Play();
    }
    
    private IEnumerator PlaySFX(AudioClip sfxClip)
    {
        AudioSource freeAudioSource = GetFreeAudioSource();
        
        if (freeAudioSource == null)
        {
            Debug.LogWarning("No free audio sources available");
            yield break;
        }
        
        freeAudioSource.clip = sfxClip;
        freeAudioSource.Play();

        yield return new WaitForSeconds(freeAudioSource.clip.length);
        
        freeAudioSource.enabled = false;
    }
    
    private AudioSource GetFreeAudioSource()
    {
        foreach (AudioSource audioSource in sfxSources)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.enabled = true;
                return audioSource;
            }
        }
        return null;
    }
}
