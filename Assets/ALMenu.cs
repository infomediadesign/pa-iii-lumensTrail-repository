using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;

public class ALMenu : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;

    private float masterVolume = 100;
    [SerializeField] private TMP_Text masterVolumeText;
    private float musicVolume = 100;
    [SerializeField] private TMP_Text musicVolumeText;
    private float sfxVolume = 100;
    [SerializeField] private TMP_Text sfxVolumeText;

    private float conversionRate = -100f;
    private float conversionMultiply = 0.8f;
    private float incrementRate = 5f;
    private float maxVolume = 100f;
    private float minVolume = 0f;

    void Start()
    {
        SetMasterVolume();
        SetMusicVolume();
        SetSFXVolume();
    }

    public void IncreaseMasterVolume() 
    {
        if (masterVolume + incrementRate <= maxVolume) masterVolume += incrementRate;
        SetMasterVolume();
    }

    public void DecreaseMasterVolume() 
    {
        if (masterVolume - incrementRate >= minVolume) masterVolume -= incrementRate;
        SetMasterVolume();
    }

    private void SetMasterVolume() 
    {
        audioMixer.SetFloat("masterVolume", VolumeConvert(masterVolume));
        masterVolumeText.text = $"{Mathf.RoundToInt(masterVolume)}";
    }

    public void IncreaseMusicVolume() 
    {
        if (musicVolume + incrementRate <= maxVolume) musicVolume += incrementRate;
        SetMusicVolume();
    }

    public void DecreaseMusicVolume() 
    {
        if (musicVolume - incrementRate >= minVolume) musicVolume -= incrementRate;
        SetMusicVolume();
    }

    private void SetMusicVolume() 
    {
        audioMixer.SetFloat("musicVolume", VolumeConvert(musicVolume));
        musicVolumeText.text = $"{Mathf.RoundToInt(musicVolume)}";
    }

    public void IncreaseSFXVolume() 
    {
        if (sfxVolume + incrementRate <= maxVolume) sfxVolume += incrementRate;
        SetSFXVolume();
    }

    public void DecreaseSFXVolume() 
    {
        if (sfxVolume - incrementRate >= minVolume) sfxVolume -= incrementRate;
        SetSFXVolume();
    }

    private void SetSFXVolume() 
    {
        audioMixer.SetFloat("sfxVolume", VolumeConvert(sfxVolume));
        sfxVolumeText.text = $"{Mathf.RoundToInt(sfxVolume)}";
    }

    private float VolumeConvert(float volume)
    {
        return (volume + conversionRate) * conversionMultiply;
    }
}
