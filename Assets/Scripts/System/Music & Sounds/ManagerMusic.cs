using UnityEngine;
using UnityEngine.Audio;

public class ManagerMusic : MonoBehaviour
{
    public static ManagerMusic instance;

    public AudioMixer mixer;
    private AudioSource musicSource;
    private AudioDistortionFilter musicDistortionFilter;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        musicSource = GetComponent<AudioSource>();
        musicDistortionFilter = GetComponent<AudioDistortionFilter>();
    }
    private void Start()
    {
        mixer.GetFloat("MusicVol", out Options.Options_Sounds.musicVolume);
        mixer.GetFloat("SFXVol", out Options.Options_Sounds.soundVolume);
    }

    public void PlayMusic(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource?.Stop();
    }

    public void SetDistortion(float amount)
    {
        musicDistortionFilter.distortionLevel = amount;
    }
    public void SetPitch (float amount)
    {
        musicSource.pitch = amount;
    }

    public void SetMusicVolume(float value)
    {
        mixer.SetFloat("MusicVol", Mathf.Log10(value) * 20);
    }
    public void SetFxVolume(float value)
    {
        mixer.SetFloat("SFXVol", Mathf.Log10(value) * 20);
    }

}
