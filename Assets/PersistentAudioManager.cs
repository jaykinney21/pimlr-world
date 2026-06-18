using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistentAudioManager : MonoBehaviour
{
    public static PersistentAudioManager Instance;
    private AudioSource audioSource;
    private AudioClip currentClip;
    private float currentPosition;
    private float currentVolume = 1f; // default to maximum volume

    public string musicPlayerSceneName = "Music Player Landscape"; // Name of the music player scene

    public GameObject MusicPlayerBoardView;

    public bool IsInHiddenCursorScene;
    public MusicPlayer musicPlayer;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.volume = currentVolume; // Initialize volume level

            // Register the scene loaded event
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Debug.Log("A duplicate PersistentAudioManager was found and is being destroyed!");
            Destroy(gameObject);
        }

        if (MusicPlayerBoardView.activeSelf)
        {
            MusicPlayerBoardView.SetActive(false);
        }
        

    }

    private void Update () 
    {
        if(Input.GetKeyDown(KeyCode.M))
        {
            if(MusicPlayerBoardView.activeSelf == false)
            {
                Scene scene = SceneManager.GetActiveScene();
                Debug.Log(" scene name "+scene.name);
                if (scene.name == "YannicksWorld") 
                {
                    IsInHiddenCursorScene = true;

                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                }
                else
                {
                    IsInHiddenCursorScene = false;
                }
            }
            else
            {
                if (IsInHiddenCursorScene)
                {
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;
                }
                else
                {
                    Cursor.visible = true;
                }
            }

            MusicPlayerBoardView.SetActive(!MusicPlayerBoardView.activeSelf);
        }
    }

    private void OnDestroy()
    {
        // Unregister the scene loaded event to avoid potential issues
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void OpenMusicPlayer () 
    {
        Scene scene = SceneManager.GetActiveScene();

        if (scene.name == "YannicksWorld")
        {
            IsInHiddenCursorScene = true;

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            IsInHiddenCursorScene = false;
        }

        MusicPlayerBoardView.SetActive(true);
    }

    public void CloseMusicPlayer()
    {
        if (IsInHiddenCursorScene)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.visible = true;
        }

        MusicPlayerBoardView.SetActive(false);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Check if the loaded scene is the music player scene
        if (scene.name == musicPlayerSceneName)
        {
            ResetAudio();
        }
    }

    public void LoadClip(AudioClip clip, float position)
    {
        currentClip = clip;
        currentPosition = position;
    }

    public void PlayAudio()
    {
        if (currentClip != null)
        {
            audioSource.clip = currentClip;
            audioSource.time = currentPosition;
            audioSource.volume = currentVolume; // Set the volume level before playing
            audioSource.Play();
        }
    }

    // Method to set the volume level
    public void SetVolume(float volume)
    {
        currentVolume = Mathf.Clamp(volume, 0f, 1f); // Ensure volume is between 0 and 1
        audioSource.volume = currentVolume;
    }

    // Method to get the current volume level
    public float GetVolume()
    {
        return currentVolume;
    }

    // Method to reset audio when returning to the music player scene
    private void ResetAudio()
    {
        audioSource.Stop();
        currentClip = null;
    }
}
