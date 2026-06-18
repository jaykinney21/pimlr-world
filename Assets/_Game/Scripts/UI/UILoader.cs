using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class UILoader : MonoBehaviour
{
    public Image backGroundimg;
    public TMPro.TextMeshProUGUI titelText;
    public TMPro.TextMeshProUGUI descriptionText;
    public Slider loadingSlider;
    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(LoadSceneAsync("YourTargetScene"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);

        // Disable scene activation to allow loading to complete
        asyncOperation.allowSceneActivation = false;

        while (!asyncOperation.isDone)
        {
            // Update the loading slider value based on the loading progress
            float progress = Mathf.Clamp01(asyncOperation.progress / 0.9f); // 0.9 is the max value for progress
            loadingSlider.value = progress;

            // Check if the loading is almost complete (progress >= 0.9)
            if (asyncOperation.progress >= 0.9f)
            {
                // Enable scene activation to complete the loading
                asyncOperation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
