using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Vuplex.WebView;

public class LoadMultiplayerScene : MonoBehaviour {
    public CanvasWebViewPrefab viewPrefab;

    // Start is called before the first frame update
    void Start () {
        viewPrefab.WaitUntilInitialized();
       // viewPrefab.InitialUrl = "https://idea-labs.xyz/humanityrocks/";
        viewPrefab.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update () {

    }

    public void LoadGameScene () {
        SceneManager.LoadScene(sceneName: "ObjectSpawn");
    }
}
