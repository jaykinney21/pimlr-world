using UnityEngine;

public class ChangeSceneOnApproach : MonoBehaviour
{
    public string sceneToLoad; // Drag and drop or type the name of the scene you want to load in the inspector.
    //public SceneManagerScript _SceneManagerScript;
    public GameMode gameMode;
    



    [Header("Assing same name --- Helix,Humanity,PLMR")]
    public GameMode targetGame;
    //private UISceneLoader sceneManagerScript;

    private void Awake()
    {
        //Debug.Log("Awake");
        
        //sceneManagerScript = GameObject.FindObjectOfType<UISceneLoader>();
    }
    private void OnTriggerEnter(Collider other)
    {

        //var _uiMAnager = GameObject.FindObjectOfType<UIManager>();
        // _UILodingScreen = _uiMAnager.UIMenus[0].UI_Gameobject.GetComponent<UILodingScreen>();
        // You can add a tag check here to ensure only specific objects (e.g., the player) can trigger the scene change.
        //Debug.Log("other ="+ other.name);
        if (other.CompareTag("Player"))
        {
            //change by abhay 
            //SceneManager.LoadScene(sceneToLoad);
            //Debug.Log("Load Scene");


            if (targetGame == GameMode.None)
            {
              
                AuthManager.Instance.currentGameMode = gameMode;
                //sceneManagerScript.LoadScene(sceneToLoad);
                SceneManagerScript.Instance.LoadScene(sceneToLoad);
            }
            else
                OpenUrlInDefaultBrowser();
            /* if (sceneToLoad == "Tuto_01b")
             {
                 Destroy(GameObject.Find("UI"));
             }*/

            //Debug.Log("End Func");
        }
    }

    [ContextMenu("OpenScene")]
    public void OpenUrlInDefaultBrowser()
    {
        //if (targetGame == GameMode.Helix)
        //{
        //    Application.OpenURL(Constant.Helixlink);
        //}
        //else if (targetGame == GameMode.HumanityRocks)
        //{
        //    Application.OpenURL(Constant.Humanitylink);
        //}
        //else if (targetGame == GameMode.Pimlr)
        //{
        //    Application.OpenURL(Constant.PLMRlink);
        //}




        AuthManager.Instance.currentGameMode = targetGame;
        if (targetGame == GameMode.Helix)
        {
            SceneManagerScript.Instance.LoadScene(Constant.Helix_Scene_Name);
        }
        else if (targetGame == GameMode.HumanityRocks)
        {
            SceneManagerScript.Instance.LoadScene(Constant.Humanity_Scene_Name);
        }
        else if (targetGame == GameMode.Pimlr)
        {
            SceneManagerScript.Instance.LoadScene(Constant.PLMR_Scene_Name);
        }

    }
}
