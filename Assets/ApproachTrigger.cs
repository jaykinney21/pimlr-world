using UnityEngine;
using UnityEngine.SceneManagement;

public class ApproachTrigger : MonoBehaviour
{
    public string sceneToLoad; // The name of the scene you want to load when player approaches

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger is tagged as "Player" 
        // Make sure your player GameObject has the tag "Player"
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(sceneToLoad); // Load the specified scene
        }
    }

    // You can remove the OnTriggerExit method if you don't need it for scene loading.
}
