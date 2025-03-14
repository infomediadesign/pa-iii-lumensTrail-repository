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
    LIGHTWAVECHARGEUP,
    LIGHTIMPULSE,
    PICKUP,
    KEKEHUNGRY,
    KEKEEATING,
    KEKEHAPPY,
    KEKERUNNING,
    BONSAIWALKING,
    BONSAISTANDINGUP,
    BONSAIEATING,
    RASSELBANDEWALKING,
    UICLICK,
    STATUENSPITZE,
    PROPELLERBLUME,
    BAUMSTAMM,
    BREAKABLEBODEN,
    LIGHTWAVECHARGEHOLD,
    LIGHTWAVEFIRE
}

[ExecuteInEditMode]
public class SoundManager : MonoBehaviour
{
    [SerializeField] private SoundList[] soundList;
    [SerializeField] private AudioClip[] Level12Music;
    [SerializeField] private AudioClip[] Level2Chase;
    [SerializeField] private AudioClip[] Level3AMusic;
    [SerializeField] private AudioClip[] Level3BMusic;
    private int level3index = 0;
    private int level12index = 0;
    private bool level3IsATrack = false;
    public static int currentLevel = 0;
    private static SoundManager instance;
    private AudioSource musicAudioSource;
    private AudioSource sfxLAudioSource;
    private AudioSource sfxNLAudioSource;
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

        sfxLAudioSource = gameObject.AddComponent<AudioSource>();
        AudioMixerGroup[] groups = audioMixer.FindMatchingGroups("SFX");
        sfxLAudioSource.outputAudioMixerGroup = groups[0];

        sfxNLAudioSource = gameObject.AddComponent<AudioSource>();
        groups = audioMixer.FindMatchingGroups("SFX");
        sfxNLAudioSource.outputAudioMixerGroup = groups[0];

        musicAudioSource = gameObject.AddComponent<AudioSource>();
        groups = audioMixer.FindMatchingGroups("Music");
        musicAudioSource.outputAudioMixerGroup = groups[0];

        musicAudioSource.loop = false;
        if (playMusicOnAwake) SoundManager.PlayLevel12Music();
    }

    private void Update()
    {
        if (currentLevel == 0)
        {
            if (instance.level12index == 1)
            {
                if (!instance.musicAudioSource.isPlaying)
                {
                    SoundManager.PlayLevel12Music();
                }
            }
        }
        else if (currentLevel == 1)
        {
            if (instance.level12index == 1)
            {
                if (!instance.musicAudioSource.isPlaying)
                {
                    SoundManager.PlayChaseMusic();
                }
            }
        }
        else 
        {
            if (!instance.musicAudioSource.isPlaying) 
            {
                SoundManager.PlayLevel3Music();
                Debug.Log("Now");
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
        if (instance.level12index == 0)
                {
                    instance.musicAudioSource.clip = instance.Level12Music[instance.level12index];
                    instance.level12index++;
                    instance.musicAudioSource.loop = false;
                    instance.musicAudioSource.Play();
                    Debug.Log(instance.musicAudioSource.clip.name);
                }
                else 
                {
                    instance.musicAudioSource.clip = instance.Level12Music[instance.level12index];
                    instance.level12index--;
                    instance.musicAudioSource.loop = true;
                    instance.musicAudioSource.Play();
                    Debug.Log(instance.musicAudioSource.clip.name);
                }
    }

    public static void StartChaseMusic()
    {
        instance.level12index = 0;
        currentLevel = 1;
        SoundManager.PlayChaseMusic();
    }

    private static void PlayChaseMusic()
    {
        if (instance.level12index == 0)
                {
                    instance.musicAudioSource.clip = instance.Level2Chase[instance.level12index];
                    instance.level12index++;
                    instance.musicAudioSource.loop = false;
                    instance.musicAudioSource.Play();
                    Debug.Log(instance.musicAudioSource.clip.name);
                }
                else 
                {
                    instance.musicAudioSource.clip = instance.Level2Chase[instance.level12index];
                    instance.level12index--;
                    instance.musicAudioSource.loop = true;
                    instance.musicAudioSource.Play();
                    Debug.Log(instance.musicAudioSource.clip.name);
                }
    }

    public static void PlayLevel3Music()
    {
        instance.musicAudioSource.loop = false;
        if (!instance.level3IsATrack) 
        {
            instance.musicAudioSource.clip = instance.Level3AMusic[instance.level3index];
            instance.level3index++;
            instance.level3IsATrack = true;
            if (instance.level3index >= 3) instance.level3index = 0;
            instance.musicAudioSource.Play();
            Debug.Log(instance.musicAudioSource.clip.name);
        }
        else 
        {
            int aOrB = UnityEngine.Random.Range(0, 9);
            Debug.Log(aOrB);
            if (aOrB % 2 == 0) 
            {
                instance.musicAudioSource.clip = instance.Level3AMusic[instance.level3index];
                instance.level3index++;
                instance.level3IsATrack = true;
                if (instance.level3index >= 3) instance.level3index = 0;
                instance.musicAudioSource.Play();
                Debug.Log(instance.musicAudioSource.clip.name);
            }
            else 
            {
                instance.musicAudioSource.clip = instance.Level3BMusic[instance.level3index];
                instance.level3index++;
                instance.level3IsATrack = false;
                if (instance.level3index >= 3) instance.level3index = 0;
                instance.musicAudioSource.Play();
                Debug.Log(instance.musicAudioSource.clip.name);
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
        instance.sfxLAudioSource.PlayOneShot(randomClip, volume);
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
        instance.sfxLAudioSource.clip = randomClip;
        instance.sfxLAudioSource.Play();
    }

    public static void PlaySoundNL(SoundType sound, float volume = 1)
    {
        AudioClip[] clips = instance.soundList[(int)sound].Sounds;
        if (clips == null || clips.Length == 0)
        {
            Debug.Log("No sounds for selected SoundType");
            return;
        }
        AudioClip randomClip = clips[UnityEngine.Random.Range(0, clips.Length)];
        instance.sfxNLAudioSource.PlayOneShot(randomClip, volume);
    }

    public static void PlaySoundLoopNL(SoundType sound, float volume = 1)
    {
        AudioClip[] clips = instance.soundList[(int)sound].Sounds;
        if (clips == null || clips.Length == 0)
        {
            Debug.Log("No sounds for selected SoundType");
            return;
        }
        AudioClip randomClip = clips[UnityEngine.Random.Range(0, clips.Length)];
        instance.sfxNLAudioSource.clip = randomClip;
        instance.sfxNLAudioSource.Play();
    }

    public static void SetSFXClipNull()
    {
        instance.sfxLAudioSource.clip = null;
    }

    public static void SetSFXNLClipNull()
    {
        instance.sfxNLAudioSource.clip = null;
    }

    public static void SwitchSoundLoop(bool input)
    {
        instance.sfxLAudioSource.loop = input;
    }

    public static void SwitchSoundLoopNL(bool input)
    {
        instance.sfxNLAudioSource.loop = input;
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