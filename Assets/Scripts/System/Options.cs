using System;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    public class Options_advanced
    {
        public const string SFX_Volume = "SFX_Volume";
        public const string MUSIC_Volume = "MUSIC_Volume";
        public const string SFX_Mute = "SFX_Mute";
        public const string MUSIC_Mute = "MUSIC_Mute";

        public const string CINEMATICS = "CINEMATICS";
        public const string LERP = "LERP";
        public const string SQUASH = "SQUASH";
        public const string SHAKE = "SHAKE";
        public const string PARTICLES = "PARTICLES";
        public const string ADV_UI = "ADV_UI";


        public static int GetIntFromBool (bool value)
        {
            return value ? 1 : 0;
        }

        public static bool GetBoolFromInt(int value)
        {
            return value > 0 ? true : false;
        }
    }


    public Slider musicUI;
    public Slider soundUI;

    public Toggle musicMuteUI;
    public Toggle soundMuteUI;

    public Toggle cinemeticsUI;
    public Toggle particlesUI;
    public Toggle advancedUI;
    public Toggle lerpUI;
    public Toggle shakeUI;
    public Toggle squashUI;

    private static Slider musicUISlider;
    private static Slider soundUISlider;


    private static Toggle musicMuteUIToggle;
    private static Toggle soundMuteUIToggle;
    private static Toggle cinemeticsUIToggle;
    private static Toggle particlesUIToggle;
    private static Toggle advancedUIToggle;
    private static Toggle lerpUIToggle;
    private static Toggle shakeUIToggle;
    private static Toggle squashUIToggle;

    public static bool cinematics = true;
    public static bool particles = true;
    public static bool advUI = true;
    public static bool lerp = true;
    public static bool music = true;
    public static bool sound = true;
    public static bool shake = true;
    public static bool squash = true;

    private static float musicVolume = 1;
    private static float sfxVolume = 1;

    private void OnEnable()
    {
        Validate();
        Initialization();
    }

    private void Initialization()
    {
        SetInitVolume();
        SetInitBooleans();
    }

    private void SetInitBooleans()
    {
        cinematics = Options_advanced.GetBoolFromInt(PlayerPrefs.GetInt(Options_advanced.CINEMATICS, 1));
        particles = Options_advanced.GetBoolFromInt(PlayerPrefs.GetInt(Options_advanced.PARTICLES, 1));
        advUI = Options_advanced.GetBoolFromInt(PlayerPrefs.GetInt(Options_advanced.ADV_UI, 1));
        lerp = Options_advanced.GetBoolFromInt(PlayerPrefs.GetInt(Options_advanced.LERP, 1));
        shake = Options_advanced.GetBoolFromInt(PlayerPrefs.GetInt(Options_advanced.SHAKE, 1));
        squash = Options_advanced.GetBoolFromInt(PlayerPrefs.GetInt(Options_advanced.SQUASH, 1));
        music = Options_advanced.GetBoolFromInt(PlayerPrefs.GetInt(Options_advanced.MUSIC_Mute, 0));
        sound = Options_advanced.GetBoolFromInt(PlayerPrefs.GetInt(Options_advanced.SFX_Mute, 0));
    }

    private static void SetInitVolume()
    {
        sfxVolume = PlayerPrefs.GetFloat(Options_advanced.SFX_Volume, 1);
        musicVolume = PlayerPrefs.GetFloat(Options_advanced.MUSIC_Volume, 1);


        soundUISlider.value = sfxVolume;
        musicUISlider.value = musicVolume;

        ManagerMusic.instance?.SetMusicVolume(musicVolume);
        ManagerMusic.instance?.SetFxVolume(sfxVolume);
    }

    private void Validate()
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
        squashUIToggle = squashUI.GetComponent<Toggle>();
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
        PlayerPrefs.SetFloat(Options_advanced.MUSIC_Volume, musicVolume);
        ManagerMusic.instance?.SetMusicVolume(musicVolume);
        if (musicVolume > 0)
        {
            musicMuteUIToggle.isOn = false;
        }
    }
    public static void SetSFXVol()
    {
        sfxVolume = soundUISlider.value;
        PlayerPrefs.SetFloat(Options_advanced.SFX_Volume, sfxVolume);
        ManagerMusic.instance?.SetFxVolume(sfxVolume);
        if (sfxVolume > 0)
        {
            soundMuteUIToggle.isOn = false;
        }
    }

    public static void SetCinematics()
    {
        cinematics = cinemeticsUIToggle.isOn;
        PlayerPrefs.SetInt(Options_advanced.CINEMATICS, Options_advanced.GetIntFromBool(cinematics));
    }

    public static void SetParticle()
    {
        particles = particlesUIToggle.isOn;
        PlayerPrefs.SetInt(Options_advanced.PARTICLES, Options_advanced.GetIntFromBool(particles));
    }

    public static void SetUI()
    {
        advUI = advancedUIToggle.isOn;
        PlayerPrefs.SetInt(Options_advanced.ADV_UI, Options_advanced.GetIntFromBool(advUI));
    }
    public static void SetLerp()
    {
        lerp = lerpUIToggle.isOn;
        PlayerPrefs.SetInt(Options_advanced.LERP, Options_advanced.GetIntFromBool(lerp));
    }

    public static void SetShake()
    {
        shake = shakeUIToggle.isOn;
        PlayerPrefs.SetInt(Options_advanced.SHAKE, Options_advanced.GetIntFromBool(shake));
    }

    public static void SetSquash()
    {
        squash = squashUIToggle.isOn;
        PlayerPrefs.SetInt(Options_advanced.SQUASH, Options_advanced.GetIntFromBool(squash));
    }
}
