using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
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
    public Toggle squashUI;

    internal class Options_advanced
    {
        public const string SOUND_Volume = "SFX_Volume";
        public const string MUSIC_Volume = "MUSIC_Volume";
        public const string SOUND_Mute = "SFX_Mute";
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
    internal class Options_Sounds
    {
        public static Slider musicUISlider;
        public static Slider soundUISlider;
        public static Toggle musicMuteUIToggle;
        public static Toggle soundMuteUIToggle;

        public static float musicVolume = 1;
        public static float soundVolume = 1;
        public static bool musicMute = true;
        public static bool soundMute = true;

        public static void Init()
        {
            musicMute = Options_advanced.GetBoolFromInt(PlayerPrefs.GetInt(Options_advanced.MUSIC_Mute, 0));
            soundMute = Options_advanced.GetBoolFromInt(PlayerPrefs.GetInt(Options_advanced.SOUND_Mute, 0));

            soundVolume = PlayerPrefs.GetFloat(Options_advanced.SOUND_Volume, 1);
            musicVolume = PlayerPrefs.GetFloat(Options_advanced.MUSIC_Volume, 1);


            soundUISlider.value = soundVolume;
            musicUISlider.value = musicVolume;
            soundMuteUIToggle.isOn = soundMute;
            musicMuteUIToggle.isOn = musicMute;

            ManagerMusic.instance?.SetMusicVolume(musicVolume);
            ManagerMusic.instance?.SetFxVolume(soundVolume);
        }

        public static void SetMusicMute()
        {
            musicMute = !musicMute;
            musicMuteUIToggle.isOn = musicMute;
            PlayerPrefs.SetInt(Options_advanced.MUSIC_Mute, Options_advanced.GetIntFromBool(musicMute));
            SetMusicVol(musicMute ? 0 : musicVolume);
        }
        public static void SetSoundMute()
        {
            soundMute = !soundMute;
            soundMuteUIToggle.isOn = soundMute;
            PlayerPrefs.SetInt(Options_advanced.SOUND_Mute, Options_advanced.GetIntFromBool(soundMute));
            SetSoundVol(soundMute ? 0 : soundVolume);
        }
        public static void SetMusicVol(float value)
        {
            musicVolume = value;
            musicUISlider.value = musicVolume;
            ManagerMusic.instance?.SetMusicVolume(musicVolume);
            PlayerPrefs.SetFloat(Options_advanced.MUSIC_Volume, musicVolume);
            if (musicVolume > 0)
            {
                musicMute = false;
                musicMuteUIToggle.isOn = musicMute;
                PlayerPrefs.SetInt(Options_advanced.MUSIC_Mute, Options_advanced.GetIntFromBool(musicMute));
            }
        }
        public static void SetSoundVol(float value)
        {
            soundVolume = value;
            soundUISlider.value = soundVolume;
            ManagerMusic.instance?.SetFxVolume(soundVolume);
            PlayerPrefs.SetFloat(Options_advanced.SOUND_Volume, soundVolume);
            if (soundVolume > 0)
            {
                soundMute = false;
                soundMuteUIToggle.isOn = soundMute;
                PlayerPrefs.SetInt(Options_advanced.SOUND_Mute, Options_advanced.GetIntFromBool(soundMute));
            }
        }
    }

    internal class Options_Graphics
    {
        public static Toggle cinemeticsUIToggle;
        public static Toggle particlesUIToggle;
        public static Toggle advancedUIToggle;
        public static Toggle lerpUIToggle;
        public static Toggle shakeUIToggle;
        public static Toggle squashUIToggle;

        public static bool cinematics = true;
        public static bool particles = true;
        public static bool advUI = true;
        public static bool lerp = true;
        public static bool shake = true;
        public static bool squash = true;

        public static void Init()
        {
            cinematics = Options_advanced.GetBoolFromInt(PlayerPrefs.GetInt(Options_advanced.CINEMATICS, 1));
            particles = Options_advanced.GetBoolFromInt(PlayerPrefs.GetInt(Options_advanced.PARTICLES, 1));
            advUI = Options_advanced.GetBoolFromInt(PlayerPrefs.GetInt(Options_advanced.ADV_UI, 1));
            lerp = Options_advanced.GetBoolFromInt(PlayerPrefs.GetInt(Options_advanced.LERP, 1));
            shake = Options_advanced.GetBoolFromInt(PlayerPrefs.GetInt(Options_advanced.SHAKE, 1));
            squash = Options_advanced.GetBoolFromInt(PlayerPrefs.GetInt(Options_advanced.SQUASH, 1));


            cinemeticsUIToggle.isOn = cinematics;
            particlesUIToggle.isOn = particles;
            advancedUIToggle.isOn = advUI;
            lerpUIToggle.isOn = lerp;
            shakeUIToggle.isOn = shake;
            squashUIToggle.isOn = squash;
        }
        public static void SetCinematics()
        {
            cinematics = Options_Graphics.cinemeticsUIToggle.isOn;
            PlayerPrefs.SetInt(Options_advanced.CINEMATICS, Options_advanced.GetIntFromBool(cinematics));
        }
        public static void SetParticle()
        {
            particles = Options_Graphics.particlesUIToggle.isOn;
            PlayerPrefs.SetInt(Options_advanced.PARTICLES, Options_advanced.GetIntFromBool(particles));
        }
        public static void SetUI()
        {
            advUI = Options_Graphics.advancedUIToggle.isOn;
            PlayerPrefs.SetInt(Options_advanced.ADV_UI, Options_advanced.GetIntFromBool(advUI));
        }
        public static void SetLerp()
        {
            lerp = Options_Graphics.lerpUIToggle.isOn;
            PlayerPrefs.SetInt(Options_advanced.LERP, Options_advanced.GetIntFromBool(lerp));
        }
        public static void SetShake()
        {
            shake = Options_Graphics.shakeUIToggle.isOn;
            PlayerPrefs.SetInt(Options_advanced.SHAKE, Options_advanced.GetIntFromBool(shake));
        }
        public static void SetSquash()
        {
            squash = Options_Graphics.squashUIToggle.isOn;
            PlayerPrefs.SetInt(Options_advanced.SQUASH, Options_advanced.GetIntFromBool(squash));
        }
    }

    private void OnEnable()
    {
        GetComponents();
        Initialization();
    }
    private void GetComponents()
    {
        Options_Sounds.soundUISlider = soundUI.GetComponent<Slider>();
        Options_Sounds.musicUISlider = musicUI.GetComponent<Slider>();
        Options_Sounds.musicMuteUIToggle = musicMuteUI.GetComponent<Toggle>();
        Options_Sounds.soundMuteUIToggle = soundMuteUI.GetComponent<Toggle>();

        Options_Graphics.cinemeticsUIToggle = cinemeticsUI.GetComponent<Toggle>();
        Options_Graphics.particlesUIToggle = particlesUI.GetComponent<Toggle>();
        Options_Graphics.advancedUIToggle = advancedUI.GetComponent<Toggle>();
        Options_Graphics.lerpUIToggle = lerpUI.GetComponent<Toggle>();
        Options_Graphics.shakeUIToggle = shakeUI.GetComponent<Toggle>();
        Options_Graphics.squashUIToggle = squashUI.GetComponent<Toggle>();
    }
    private void Initialization()
    {
        Options_Sounds.Init();
        Options_Graphics.Init();
    }

    public static void SetMusicMute()
    {
        Options_Sounds.SetMusicMute();
    }
    public static void SetSoundMute()
    {
        Options_Sounds.SetSoundMute();
    }
    public static void SetMusicVolume()
    {
        Options_Sounds.SetMusicVol(Options_Sounds.musicUISlider.value);
    }
    public static void SetSoundVolume()
    {
        Options_Sounds.SetSoundVol(Options_Sounds.soundUISlider.value);
    }

    public static void SetCinematics()
    {
        Options_Graphics.SetCinematics();
    }

    public static void SetParticle()
    {
        Options_Graphics.SetParticle();
    }

    public static void SetUI()
    {
        Options_Graphics.SetUI();
    }
    public static void SetLerp()
    {
        Options_Graphics.SetLerp();
    }

    public static void SetShake()
    {
        Options_Graphics.SetShake();
    }

    public static void SetSquash()
    {
        Options_Graphics.SetSquash();
    }
}
