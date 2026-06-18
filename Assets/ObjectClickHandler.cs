using UnityEngine;
using UnityEngine.Video;

public class ObjectClickHandler : MonoBehaviour
{
    public GameObject objectToDeactivate;
    public GameObject videoPlayerObject;

    private VideoPlayer videoPlayer;

    private void Start()
    {
        // Get the VideoPlayer component from the video player object
        videoPlayer = videoPlayerObject.GetComponent<VideoPlayer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the other object has the player tag
        if (other.gameObject.CompareTag("Television"))
        {
            // Deactivate the specified game object
            objectToDeactivate.SetActive(false);

            // Activate the video player and play the video
            videoPlayerObject.SetActive(true);
            videoPlayer.Play();
        }
    }
}
