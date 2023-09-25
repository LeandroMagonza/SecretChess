using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMusic : MonoBehaviour
{
    public AudioClip music;
    private void OnEnable()
    {
        ManagerMusic.instance?.PlayMusic(music);
    }
}
