using System.Collections.Generic;
using UnityEngine;

public class MusicLibrary : MonoBehaviour
{
    public List<Artist> Artists;
}

[System.Serializable]
public class Artist
{
    public string ArtistName;
    public List<Album> Albums;
}

[System.Serializable]
public class Album
{
    public string AlbumName;
    public Sprite AlbumSprite;
    //public List<AudioClip> Songs;
    public List<SoundTracks> songClips_Bools;
    //public SoundTracks sound;

}

[System.Serializable]
public class SoundTracks
{
    public AudioClip song;
    public bool unlocked;
    
}
