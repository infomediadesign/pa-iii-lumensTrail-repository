using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Audio;

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
        musicAudioSource.loop = true;
        if (playMusicOnAwake) SoundManager.PlayMusic(0, 1);
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