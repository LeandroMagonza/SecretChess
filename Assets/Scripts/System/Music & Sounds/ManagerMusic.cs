using UnityEngine;
using UnityEngine.Audio;

public class ManagerMusic : MonoBehaviour
{
    public static ManagerMusic instance;
    public AudioMixer mixer;
    private AudioSource musicSource;
    private AudioDistortionFilter musicDistortionFilter;
    private AudioSource helper;
    private float musicDefault;
    private float sfxDefault;
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
        helper = GetComponentInChildren<AudioSource>();
        musicDistortionFilter = GetComponent<AudioDistortionFilter>();
    }
    //private void Start()
    //{
    //    mixer.GetFloat("MusicVol", out musicDefault);
    //    mixer.GetFloat("SFXVol", out sfxDefault);
    //}

    public void PlayMusic(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.Play();
    }
    public void PlayInHelper(AudioClip clip)
    {
        helper.PlayOneShot(clip);
    }
    public void StopHelper()
    {
        helper?.Stop();
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
