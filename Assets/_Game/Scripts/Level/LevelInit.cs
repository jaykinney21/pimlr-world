using UnityEngine;

public class LevelInit : MonoBehaviour
{
    // Settings
    [Space(5)]
    [Header("Settings")]
    public string showMenuAtStart = "";
    public bool createUI;
    public bool create_PIMLR_UI;
    public bool createPlayerManager;
    public bool createEnumManager;
    public bool authManager;

    void Awake()
    {
        // Create UI
        if (createUI && SceneManagerScript.Instance.uiManager == null) InstantiatePrefab<UIManager>("UI");

        // Create PIMLR_UI
        if (create_PIMLR_UI) InstantiatePrefab<UIManager>("PIMLR_UI");

        // Create PlayerManager
        //if (createPlayerManager) InstantiatePrefab<PlayerManager>("PlayerManager");

        //// Create EnumManager
        //if (createEnumManager) InstantiatePrefab<EnumManager>("EnumManager");

        // Create AuthManager
        if (authManager) InstantiatePrefab<AuthManager>("AuthManager");

        // Open a menu at level start
        if (createUI && !string.IsNullOrEmpty(showMenuAtStart))
            ShowMenuAtStart();

        // Open a menu at level start for PIMLR_UI
        if (create_PIMLR_UI && !string.IsNullOrEmpty(showMenuAtStart))
            ShowMenuAtStart();
    }

    // Show menu at start
    void ShowMenuAtStart()
    {
        //Debug.Log("showMenuAtStart =" + showMenuAtStart);
        FindObjectOfType<UIManager>().ShowMenu(showMenuAtStart);
    }

    // Instantiate prefab if not found
    void InstantiatePrefab<T>(string prefabName) where T : Component
    {
        if (!GameObject.FindObjectOfType<T>())
        {
            GameObject.Instantiate(Resources.Load(prefabName), Vector3.zero, Quaternion.identity);
        }
    }
}
