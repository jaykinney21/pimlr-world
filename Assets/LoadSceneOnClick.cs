using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneOnClick : MonoBehaviour
{

    // This function can be called by a UI Button's OnClick event.
    public void LoadThirdPersonShooterDemoScene()
    {
        // Load the scene named "ThirdPerson Shooter Demo".
        SceneManager.LoadScene("ThirdPerson Shooter Demo");
    }

    public void LoadHelixCar()
    {
        // Load the scene named "ThirdPerson Shooter Demo".
        SceneManager.LoadScene("Path Follow_Ash");
    }

    public void loadPIMLGame()
    {
        // Somewhere in your Scene 1 script, when about to change scenes:
      /*  AudioSource yourAudioSource = yourAudioSource = GameObject.FindGameObjectWithTag("audiosourcetag").GetComponent<AudioSource>();
        ;
        *//* the AudioSource from your Canvas GameObject *//*
        ;
        PersistentAudioManager.Instance.LoadClip(yourAudioSource.clip, yourAudioSource.time);*/ // Prashant Comment 31-10
        SceneManager.LoadScene("YannicksWorld");

    }
}
