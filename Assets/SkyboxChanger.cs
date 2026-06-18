using UnityEngine;
using UnityEngine.UI; // Required for Unity's built-in UI
// using TMPro; // Uncomment this if you're using TextMeshPro

public class SkyboxChanger : MonoBehaviour
{
    [System.Serializable]
    public class SkyboxFaction
    {
        public Material skybox;
        public string factionName;
    }

    public SkyboxFaction[] skyboxFactions;
    private int currentSkyboxIndex = 0;

    public Text factionDisplayText; // Drag and drop your Text component here
    // public TMP_Text factionDisplayText; // Uncomment this if you're using TextMeshPro

    private void Start()
    {
        UpdateSkyboxAndFaction();
    }

    public void ChangeSkybox()
    {
        currentSkyboxIndex++;
        if (currentSkyboxIndex >= skyboxFactions.Length)
        {
            currentSkyboxIndex = 0;
        }
        UpdateSkyboxAndFaction();
    }

    private void UpdateSkyboxAndFaction()
    {
        RenderSettings.skybox = skyboxFactions[currentSkyboxIndex].skybox;
        DynamicGI.UpdateEnvironment(); // Update global illumination

        string currentFaction = skyboxFactions[currentSkyboxIndex].factionName;
        factionDisplayText.text = "Faction: " + currentFaction; // Set the faction name to the Text component in the desired format
    }
}
