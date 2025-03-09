using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;

public class ALMenu : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;

    private float masterVolume = 80f;
    [SerializeField] private TMP_Text masterVolumeText;
    private float musicVolume = 80f;
    [SerializeField] private TMP_Text musicVolumeText;
    private float sfxVolume = 80f;
    [SerializeField] private TMP_Text sfxVolumeText;

    private float conversionRate = 80f;
    private float incrementRate = 5f;

    public void IncreaseMasterVolume() 
    {
        masterVolume += incrementRate;
        audioMixer.SetFloat("masterVolume", masterVolume);
    }

}
