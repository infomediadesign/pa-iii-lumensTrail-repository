using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Audio;
using UnityEngine.AI;
using Unity.Mathematics;

public enum SoundType
{
    BACKGROUNDMUSIC,
    WALKING,
    JUMPING,
    LANDING,
    LIGHTTHROW,
    LIGHTWAVE,
    LIGHTIMPULSE,
    PICKUP
}

[ExecuteInEditMode]
public class SoundManager : MonoBehaviour
{
    [SerializeField] private SoundList[] soundList;
    [SerializeField] private AudioClip[] Level1Music;
    [SerializeField] private AudioClip[] Level2Music;
    [SerializeField] private AudioClip[] Level3AMusic;
    [SerializeField] private AudioClip[] Level3BMusic;
    private int level3index = 0;
    private int level12index = 0;
    private bool level3IsATrack = false;
    private int currentLevel = 1;
    private static SoundManager instance;
    private AudioSource musicAudioSource;
    private AudioSource sfxAudioSource;
    [SerializeField] private AudioMixer audioMixer;

    [Tooltip("Play the first Sound File in the Background Music tab on Game Start")]
    [SerializeField] private bool playMusicOnAwake = false;

    private void Awake()
    {
        if (!Application.isPlaying) return;

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    private void Start()
    {
        if (!Application.isPlaying) return;

        sfxAudioSource = gameObject.AddComponent<AudioSource>();
        AudioMixerGroup[] groups = audioMixer.FindMatchingGroups("SFX");
        sfxAudioSource.outputAudioMixerGroup = groups[0];
        musicAudioSource = gameObject.AddComponent<AudioSource>();
        groups = audioMixer.FindMatchingGroups("Music");
        musicAudioSource.outputAudioMixerGroup = groups[0];
        musicAudioSource.loop = false;
        if (playMusicOnAwake) SoundManager.PlayLevel12Music();
    }

    private void Update()
    {
        if (instance.currentLevel < 2)
        {
            if (instance.level12index == 1)
            {
                if (!instance.musicAudioSource.isPlaying)
                {
                    SoundManager.PlayLevel12Music();
                }
            }
        }
    }

    public static void PlayMusic(int index, float volume = 1)
    {
        AudioClip[] clips = instance.soundList[(int)SoundType.BACKGROUNDMUSIC].Sounds;
        if (index > clips.Length)
        {
            Debug.Log("Index bigger than background music array size");
            return;
        }
        instance.musicAudioSource.clip = clips[index];
        instance.musicAudioSource.volume = volume;
        instance.musicAudioSource.Play();
    }

    public static void PlayLevel12Music()
    {
        switch(instance.currentLevel)
        {
            case 0:
                if (instance.level12index == 0)
                {
                    instance.musicAudioSource.clip = instance.Level1Music[instance.level12index];
                    instance.level12index++;
                    instance.musicAudioSource.loop = false;
                    instance.musicAudioSource.Play();
                }
                else 
                {
                    instance.musicAudioSource.clip = instance.Level1Music[instance.level12index];
                    instance.level12index--;
                    instance.musicAudioSource.loop = true;
                    instance.currentLevel++;
                    instance.musicAudioSource.Play();
                }
                break;
            case 1:
                if (instance.level12index == 0)
                {
                    instance.musicAudioSource.clip = instance.Level2Music[instance.level12index];
                    instance.level12index++;
                    instance.musicAudioSource.loop = false;
                    instance.musicAudioSource.Play();
                }
                else 
                {
                    instance.musicAudioSource.clip = instance.Level2Music[instance.level12index];
                    instance.level12index--;
                    instance.musicAudioSource.loop = true;
                    instance.currentLevel++;
                    instance.musicAudioSource.Play();
                }
                break;
            default:
                break;
        }
    }

    public static void PlayLevel3Music()
    {
        if (!instance.level3IsATrack) 
        {
            instance.musicAudioSource.clip = instance.Level3AMusic[instance.level3index];
            instance.level3index++;
            instance.level3IsATrack = true;
            if (instance.level3index >= 2) instance.level3index = 0;
            instance.musicAudioSource.Play();
        }
        else 
        {
            int aOrB = UnityEngine.Random.Range(0, 1);
            if (aOrB == 0) 
            {
                instance.musicAudioSource.clip = instance.Level3AMusic[instance.level3index];
                instance.level3index++;
                instance.level3IsATrack = true;
                if (instance.level3index >= 2) instance.level3index = 0;
                instance.musicAudioSource.Play();
            }
            else 
            {
                instance.musicAudioSource.clip = instance.Level3BMusic[instance.level3index];
                instance.level3index++;
                instance.level3IsATrack = false;
                if (instance.level3index >= 2) instance.level3index = 0;
                instance.musicAudioSource.Play();
            }
        }
    }

    public static void StopMusic()
    {
        instance.musicAudioSource.Stop();
    }

    public static void PlaySound(SoundType sound, float volume = 1)
    {
        AudioClip[] clips = instance.soundList[(int)sound].Sounds;
        if (clips == null || clips.Length == 0)
        {
            Debug.Log("No sounds for selected SoundType");
            return;
        }
        AudioClip randomClip = clips[UnityEngine.Random.Range(0, clips.Length)];
        instance.sfxAudioSource.PlayOneShot(randomClip, volume);
    }

    public static void PlaySoundLoop(SoundType sound, float volume = 1)
    {
        AudioClip[] clips = instance.soundList[(int)sound].Sounds;
        if (clips == null || clips.Length == 0)
        {
            Debug.Log("No sounds for selected SoundType");
            return;
        }
        AudioClip randomClip = clips[UnityEngine.Random.Range(0, clips.Length)];
        instance.sfxAudioSource.clip = randomClip;
        instance.sfxAudioSource.Play();
    }

    public static void SetSFXClipNull()
    {
        instance.sfxAudioSource.clip = null;
    }

    public static void SwitchSoundLoop(bool input)
    {
        instance.sfxAudioSource.loop = input;
    }

#if UNITY_EDITOR
    private void OnEnable()
    {
        string[] names = Enum.GetNames(typeof(SoundType));
        Array.Resize(ref soundList, names.Length);
        for (int i = 0; i < soundList.Length; i++)
        {
            soundList[i].name = names[i];
        }
    }
#endif
}

[Serializable]
public struct SoundList
{
    public AudioClip[] Sounds { get => sounds; }
    [HideInInspector] public string name;
    [SerializeField] private AudioClip[] sounds;
}