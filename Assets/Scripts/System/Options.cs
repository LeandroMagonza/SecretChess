using UnityEngine;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    public Slider musicUI;
    public Slider soundUI;

    private static Slider musicUISlider;
    private static Slider soundUISlider;

    public static bool cinematics = true;
    public static bool particles = true;
    public static bool advUI = true;
    public static bool music = true;
    public static bool sound = true;

    public static float musicVolume = 1;
    public static float sfxVolume = 1;

    private void OnEnable()
    {
        soundUISlider = soundUI.GetComponent<Slider>();
        musicUISlider = musicUI.GetComponent<Slider>();
        ManagerMusic.instance?.SetMusicVolume(musicVolume);
        ManagerMusic.instance?.SetFxVolume(sfxVolume);
    }

    public static void SetMute(bool value)
    {
        if (value)
        {
            ManagerMusic.instance?.SetMusicVolume(0);
            ManagerMusic.instance?.SetFxVolume(0);
        }
        else
        {
            ManagerMusic.instance?.SetMusicVolume(musicVolume);
            ManagerMusic.instance?.SetFxVolume(sfxVolume);
        }

    }

    public static void SetMusicEnable(bool value)
    {
        if (value) 
            ManagerMusic.instance?.SetMusicVolume(0);
        else
            ManagerMusic.instance?.SetMusicVolume(musicVolume);
    }

    public static void SetSoundEnable(bool value)
    {
        if (value)
            ManagerMusic.instance?.SetFxVolume(0);
        else
            ManagerMusic.instance?.SetFxVolume(sfxVolume);
    }

    public static void SetMusicVolume()
    {
        musicVolume = musicUISlider.value;
        ManagerMusic.instance?.SetMusicVolume(musicVolume);
    }

    public static void SetSFXVolume()
    {
        sfxVolume = soundUISlider.value;
        ManagerMusic.instance?.SetFxVolume(sfxVolume);
    }

    public static void SetCinematics(bool value)
    {
        cinematics = value;
    }

    public static void SetEffects(bool value)
    {
        particles = value;
    }

    public static void SetUI(bool value)
    {
        advUI = value;
    }
}
