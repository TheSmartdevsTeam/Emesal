using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public bool Subtitles;
    public bool Fullscreen;
    public int ResolutionX;
    public int ResolutionY;
    public float Gamma;
    public float MasterVolume;
    public float MusicVolume;
    public float SpeachVolume;

    public GameData()
    {
        Subtitles = false;
        Fullscreen = true;
        ResolutionX = 1280;
        ResolutionX = 720;
        Gamma = 0.5f;
        MasterVolume = 1;
        MusicVolume = 1;
        SpeachVolume = 1;
    }

    

}
