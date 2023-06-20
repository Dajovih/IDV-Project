using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Audio/AudioLibrary")]
public class AudioLibrary : ScriptableObject
{
    public AudioMusicClip[] music;
    public AudioSoundGroup[] soundGroups;
}


public enum AudioMusicType { Menu, Level_1, Level_2, Level_3, Death, Victory, Score }

[Serializable]
public class AudioMusicClip
{
    public AudioMusicType MusicType;
    public AudioClip Clip;
}

[Serializable]
public class AudioSoundGroup
{
    public string groupID;
    public AudioClip[] clips;
}
