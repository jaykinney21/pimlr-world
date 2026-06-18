using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Video;

// PIMLR #9: Reusable full-motion-video (FMV) helper for between-level cutscenes.
// Attach to a GameObject that has (or references) a VideoPlayer. Assign either a VideoClip
// or a StreamingAssets file name, then call Play() to run the clip once. When the clip finishes
// (loopPointReached) — or the safety timeout elapses — it invokes onFinished and, optionally,
// loads the next scene via SceneManagerScript.
//
// Editor wiring (residual): drop this on an FMV canvas object, assign 'videoPlayer' (and its
// RawImage/RenderTexture target), tick 'loadSceneOnFinish' + set 'nextSceneName' if you want it
// to advance automatically, or leave that off and subscribe to onFinished in code.
[RequireComponent(typeof(VideoPlayer))]
public class FMVSequencer : MonoBehaviour
{
    [Header("Source (clip wins; else StreamingAssets file)")]
    [Tooltip("PIMLR #9: VideoPlayer used to render the FMV. Defaults to a VideoPlayer on this GameObject.")]
    [SerializeField] private VideoPlayer videoPlayer;
    [Tooltip("PIMLR #9: Optional explicit clip. If null, streamingAssetsFileName is used.")]
    [SerializeField] private VideoClip videoClip;
    [Tooltip("PIMLR #9: StreamingAssets file used when no videoClip is assigned.")]
    [SerializeField] private string streamingAssetsFileName = "PLMRVideo.mp4";

    [Header("Behavior")]
    [Tooltip("PIMLR #9: Play automatically in Start().")]
    [SerializeField] private bool playOnStart = false;
    [Tooltip("PIMLR #9: GameObject (e.g. the FMV RawImage/canvas) toggled on while playing.")]
    [SerializeField] private GameObject screenRoot;
    [Tooltip("PIMLR #9: Safety cap (seconds) so a stuck video can never block the flow.")]
    [SerializeField] private float maxWaitSeconds = 60f;

    [Header("On finish")]
    [Tooltip("PIMLR #9: When true, loads nextSceneName via SceneManagerScript after the clip ends.")]
    [SerializeField] private bool loadSceneOnFinish = false;
    [Tooltip("PIMLR #9: Scene name to load on finish (used only when loadSceneOnFinish is true).")]
    [SerializeField] private string nextSceneName = "SceneStaticEU";

    // PIMLR #9: Subscribe in code to react when the FMV completes.
    public event Action onFinished;

    public bool IsPlaying { get; private set; }

    private void Awake()
    {
        if (videoPlayer == null) videoPlayer = GetComponent<VideoPlayer>();
    }

    private void Start()
    {
        if (playOnStart) Play();
    }

    // PIMLR #9: Play the FMV once, then fire onFinished (and optionally load the next scene).
    public void Play()
    {
        Play(null);
    }

    // PIMLR #9: Play the FMV once with an extra one-shot completion callback.
    public void Play(Action completed)
    {
        if (IsPlaying) return;
        StartCoroutine(PlayRoutine(completed));
    }

    private IEnumerator PlayRoutine(Action completed)
    {
        if (videoPlayer == null)
        {
            Debug.LogWarning("FMVSequencer: no VideoPlayer assigned.");
            HandleFinished(completed);
            yield break;
        }

        IsPlaying = true;

        if (screenRoot != null) screenRoot.SetActive(true);

        if (videoClip != null)
        {
            videoPlayer.source = VideoSource.VideoClip;
            videoPlayer.clip = videoClip;
        }
        else
        {
            videoPlayer.source = VideoSource.Url;
            videoPlayer.url = Application.streamingAssetsPath + "/" + streamingAssetsFileName;
        }

        videoPlayer.isLooping = false;

        bool finished = false;
        VideoPlayer.EventHandler onLoopPoint = (vp) => { finished = true; };
        videoPlayer.loopPointReached += onLoopPoint;

        videoPlayer.Prepare();
        float prepTimeout = Time.realtimeSinceStartup + 5f;
        while (!videoPlayer.isPrepared && Time.realtimeSinceStartup < prepTimeout)
            yield return null;

        videoPlayer.Play();

        float deadline = Time.realtimeSinceStartup + Mathf.Max(1f, maxWaitSeconds);
        while (!finished && Time.realtimeSinceStartup < deadline)
        {
            if (videoPlayer.isPrepared && !videoPlayer.isPlaying && videoPlayer.frame > 0)
                break;

            yield return null;
        }

        videoPlayer.loopPointReached -= onLoopPoint;
        videoPlayer.Stop();

        if (screenRoot != null) screenRoot.SetActive(false);

        IsPlaying = false;
        HandleFinished(completed);
    }

    private void HandleFinished(Action completed)
    {
        completed?.Invoke();
        onFinished?.Invoke();

        if (loadSceneOnFinish && SceneManagerScript.Instance != null)
        {
            SceneManagerScript.Instance.LoadScene(nextSceneName);
        }
    }
}
