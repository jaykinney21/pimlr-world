using UnityEngine;
using UnityEngine.SceneManagement;

public class ActivationManager : MonoBehaviour
{
    public string audioSourceTag = "audiosourcetag"; // Tag of the GameObject you're looking for
    private GameObject targetGameObject; // The GameObject you want to activate/deactivate

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Find the GameObject by its tag
        targetGameObject = GameObject.FindGameObjectWithTag(audioSourceTag);

        if (targetGameObject != null)
        {
            targetGameObject.SetActive(false); // Deactivate the GameObject

            // Example: Activate the GameObject after 3 seconds
            // This is optional and only for demonstration. 
            // You can replace this with any other logic.
            Invoke("ActivateGameObject", 3f);
        }
        else
        {
            Debug.LogWarning($"No GameObject found with the tag: {audioSourceTag}");
        }
    }

    private void ActivateGameObject()
    {
        if (targetGameObject != null)
            targetGameObject.SetActive(true); // Activate the GameObject
    }
}
