using UnityEngine;
using UnityEngine.Audio;

/// <summary>

/// </summary>
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Grupos del Mixer")]
    public UnityEngine.Audio.AudioMixerGroup musicGroup;
    public UnityEngine.Audio.AudioMixerGroup sfxGroup;
    
    [Header("Mixer")]
    public AudioMixer mixer;

    [Header("Música")]
    public AudioSource musicSource;
    public AudioClip defaultMusic;

    private const string MusicParam = "MusicVolume";
    private const string SFXParam = "SFXVolume";

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Carga el volumen guardado (o 0.75 por defecto la primera vez)
        SetMusicVolume(PlayerPrefs.GetFloat("MusicVolume", 0.75f));
        SetSFXVolume(PlayerPrefs.GetFloat("SFXVolume", 0.75f));
    }

    private void Start()
    {
        if (defaultMusic != null) PlayMusic(defaultMusic);
    }

    public void PlayMusic(AudioClip clip, bool loop = true)
    {
        if (musicSource.clip == clip && musicSource.isPlaying) return; // ya está sonando, no reinicies

        musicSource.clip = clip;
        musicSource.loop = loop;
        musicSource.Play();
    }

    /// <summary>value va de 0 (mute) a 1 (volumen máximo)</summary>
    public void SetMusicVolume(float value)
    {
        float dB = ToDecibels(value);
        mixer.SetFloat(MusicParam, dB);
        PlayerPrefs.SetFloat("MusicVolume", value);
    }

    public void SetSFXVolume(float value)
    {
        float dB = ToDecibels(value);
        mixer.SetFloat(SFXParam, dB);
        PlayerPrefs.SetFloat("SFXVolume", value);
    }

    private float ToDecibels(float linearValue)
    {
        // Los sliders van de 0 a 1, pero el Mixer trabaja en decibeles (log)
        return linearValue <= 0.0001f ? -80f : Mathf.Log10(linearValue) * 20f;
    }
}