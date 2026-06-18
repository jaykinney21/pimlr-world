using System.Collections;
using System.Collections.Generic;
using TS.Generics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class ChangeSceneOnTrigger : MonoBehaviour
{

    [SerializeField] GameObject loadingbar;

    [SerializeField] VideoPlayer videoPlayer;

    [SerializeField] GameObject skipbtn;
    // Start is called before the first frame update


    private void Awake()
    {
        if (videoPlayer != null)
        {
            Debug.Log("Start");

            if (Application.platform == RuntimePlatform.WebGLPlayer || Application.platform == RuntimePlatform.WindowsEditor)
                videoPlayer.url = System.IO.Path.Combine(Application.streamingAssetsPath, "Stryker_14.mp4");

            videoPlayer.Play();

            videoPlayer.loopPointReached += VideoCompleted;

        }
    }


    public void OnSkipVideoBtnClick()
    {
        videoCompleted = true;
    }
    private void Start()
    {
        if (SceneManager.GetActiveScene().name != "NeonCityScene")
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            StartCoroutine(LoadNextAsyncSceneNeonityScene());
        }


        if (videoPlayer != null)
        {
            StartCoroutine(LoadNextAsyncSceneCutScene());
        }


    }

    bool videoCompleted = false;
    void VideoCompleted(VideoPlayer vp)
    {
        //Debug.Log("VIDEO COMPLETED");
        // loadingbar.SetActive(true);
        videoCompleted = true;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (SceneManager.GetActiveScene().name != "NeonCityScene")
            {
                loadingbar.SetActive(true);
                other.gameObject.SetActive(false);

                StartCoroutine(LoadNextAsyncScene());
            }
            else
            {
                nextSceneAllowed = true;
            }
        }
        else
            Debug.Log(other.name + "::::" + other.tag);
    }
    IEnumerator LoadNextAsyncSceneCutScene()
    {  // The Application loads the Scene in the background as the current Scene runs.
       // This is particularly good for creating loading screens.
       // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
       // a sceneBuildIndex of 1 as shown in Build Settings.

        //Debug.Log(":::::::::::>>>>>"+(SceneManager.GetActiveScene().buildIndex + 1));
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync((SceneManager.GetActiveScene().buildIndex + 1));
        asyncLoad.allowSceneActivation = false;


        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            if (asyncLoad.progress > 0.85f && !skipbtn.activeSelf)
            {
                skipbtn.SetActive(true);
            }

            asyncLoad.allowSceneActivation = videoCompleted;
            //Debug.Log("videoCompleted: " + videoCompleted);
            yield return null;
        }
    }

    bool nextSceneAllowed=false;
    IEnumerator LoadNextAsyncSceneNeonityScene()
    {  // The Application loads the Scene in the background as the current Scene runs.
       // This is particularly good for creating loading screens.
       // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
       // a sceneBuildIndex of 1 as shown in Build Settings.

        //Debug.Log(":::::::::::>>>>>"+(SceneManager.GetActiveScene().buildIndex + 1));
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync((SceneManager.GetActiveScene().buildIndex + 1));
        asyncLoad.allowSceneActivation = false;


        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {

            asyncLoad.allowSceneActivation = nextSceneAllowed;
            //Debug.Log("videoCompleted: " + videoCompleted);
            yield return null;
        }
    }




    IEnumerator LoadNextAsyncScene()
    {  // The Application loads the Scene in the background as the current Scene runs.
       // This is particularly good for creating loading screens.
       // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
       // a sceneBuildIndex of 1 as shown in Build Settings.

        //Debug.Log(":::::::::::>>>>>"+(SceneManager.GetActiveScene().buildIndex + 1));
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync((SceneManager.GetActiveScene().buildIndex + 1));

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

}
