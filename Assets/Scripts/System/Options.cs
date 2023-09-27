using UnityEngine;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    public Slider musicUI;
    public Slider soundUI;

    public Toggle musicMuteUI;
    public Toggle soundMuteUI;

    public Toggle cinemeticsUI;
    public Toggle particlesUI;
    public Toggle advancedUI;
    public Toggle lerpUI;
    public Toggle shakeUI;

    private static Slider musicUISlider;
    private static Slider soundUISlider;


    private static Toggle musicMuteUIToggle;
    private static Toggle soundMuteUIToggle;
    private static Toggle cinemeticsUIToggle;
    private static Toggle particlesUIToggle;
    private static Toggle advancedUIToggle;
    private static Toggle lerpUIToggle;
    private static Toggle shakeUIToggle;

    public static bool cinematics = true;
    public static bool particles = true;
    public static bool advUI = true;
    public static bool lerp = true;
    public static bool music = true;
    public static bool sound = true;
    public static bool shake = true;

    private static float musicVolume = 1;
    private static float sfxVolume = 1;

    private void OnEnable()
    {
        soundUISlider = soundUI.GetComponent<Slider>();
        musicUISlider = musicUI.GetComponent<Slider>();

        musicMuteUIToggle = musicMuteUI.GetComponent<Toggle>();
        soundMuteUIToggle = soundMuteUI.GetComponent<Toggle>();
        cinemeticsUIToggle = cinemeticsUI.GetComponent<Toggle>();
        particlesUIToggle = particlesUI.GetComponent<Toggle>();
        advancedUIToggle = advancedUI.GetComponent<Toggle>();
        lerpUIToggle = lerpUI.GetComponent<Toggle>();
        shakeUIToggle = shakeUI.GetComponent<Toggle>();

        ManagerMusic.instance?.SetMusicVolume(musicVolume);
        ManagerMusic.instance?.SetFxVolume(sfxVolume);

        Debug.Log(musicVolume);
    }

    public static void SetMusicMute()
    {
        if (musicMuteUIToggle.isOn)
        {
            ManagerMusic.instance?.SetMusicVolume(0);
            musicUISlider.value = 0;
        }
        else
        {
            ManagerMusic.instance?.SetMusicVolume(musicVolume);
            musicUISlider.value = musicVolume;
        }
    }

    public static void SetSoundMute()
    {
        if (soundMuteUIToggle.isOn)
        {
            ManagerMusic.instance?.SetFxVolume(0);
            soundUISlider.value = 0;
        }
        else
        {
            ManagerMusic.instance?.SetFxVolume(sfxVolume);
            soundUISlider.value = sfxVolume;
        }
    }

    public static void SetMusicVol()
    {
        musicVolume = musicUISlider.value;
        ManagerMusic.instance?.SetMusicVolume(musicVolume);
        if (musicVolume > 0)
        {
            musicMuteUIToggle.isOn = false;
        }
    }

    public static void SetSFXVol()
    {
        sfxVolume = soundUISlider.value;
        ManagerMusic.instance?.SetFxVolume(sfxVolume);
        if (sfxVolume > 0)
        {
            soundMuteUIToggle.isOn = false;
        }
    }

    public static void SetCinematics()
    {
        cinematics = cinemeticsUIToggle.isOn;
    }

    public static void SetParticle()
    {
        particles = particlesUIToggle.isOn;
    }

    public static void SetUI()
    {
        advUI = advancedUIToggle.isOn;
    }
    public static void SetLerp()
    {
        lerp = lerpUIToggle.isOn;
    }

    public static void SetShake()
    {
        shake = shakeUIToggle.isOn;
    }
}
