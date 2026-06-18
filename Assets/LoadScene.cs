using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    // This function can be called by a UI Button's OnClick event.
    public void LoadUsernameScene()
    {
        // Load the scene named "ThirdPerson Shooter Demo".
        SceneManager.LoadScene("EnterUsername");
    }

    public void LoadHelixUsernameScene()
    {
        // Load the scene named "ThirdPerson Shooter Demo".
        SceneManager.LoadScene("EnterUsername Helix");
    }

    public void LoadPlaneUsernameScene () 
    {
        SceneManager.LoadScene("EnterUsernameAeroplane");
    }

    public void LoadConfigScene()
    {
        // Load the scene named "ThirdPerson Shooter Demo".
        SceneManager.LoadScene("SceneStaticEU");
    }

    public void LoadHelixConfigScene()
    {
        // Load the scene named "ThirdPerson Shooter Demo".
        //SceneManager.LoadScene("SceneStaticEU Helix");
        SceneManager.LoadScene("FirstScene");
    }

    public void LoadPlaneRaceScene () 
    {
        SceneManager.LoadScene("Tuto_01b");
    } 
    
    public void LoadIDEAMainLandScene() 
    {
        SceneManager.LoadScene("IDEAMainLand");
    }
}
