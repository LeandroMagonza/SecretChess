using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMusicStart : MonoBehaviour
{
    public AudioClip music;
    private void Start()
    {
        ManagerMusic.instance?.PlayMusic(music);
    }
}
