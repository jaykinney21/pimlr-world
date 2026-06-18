using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class UIVideoScreen : MonoBehaviour
{
    public Video[] _videoClips;

    public VideoPlayer videoPlayer;



    void Start()
    {
        videoPlayer.url = Application.streamingAssetsPath + "/PLMRVideo.mp4";
        // Make sure you have assigned the VideoPlayer in the Inspector or via code
        videoPlayer.loopPointReached += OnVideoFinished;
    }
    void OnVideoFinished(VideoPlayer vp)
    {
        Debug.Log("Video Finished Once!");
        SceneManagerScript.Instance.LoadScene("SceneStaticEU");
    }
}
[System.Serializable]
public class Video
{
    public VideoClip _videoClip;
}
