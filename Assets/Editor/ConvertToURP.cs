using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ConvertToURP : Editor
{
    [MenuItem("Tools/Convert to URP")]
    private static void ConvertAllMaterialsToURP()
    {
        // Find all materials in the scene
        Renderer[] renderers = GameObject.FindObjectsOfType<Renderer>();

        foreach (Renderer renderer in renderers)
        {
            Material[] materials = renderer.sharedMaterials;

            for (int i = 0; i < materials.Length; i++)
            {
                Material oldMat = materials[i];
                Material newMat = new Material(Shader.Find("Universal Render Pipeline/Lit"));

                // Copy properties from the old material to the new one (you might need to set more properties)
                newMat.SetColor("_BaseColor", oldMat.GetColor("_Color"));
                newMat.SetTexture("_BaseMap", oldMat.GetTexture("_MainTex"));

                // Assign the new material to the renderer
                materials[i] = newMat;
            }

            renderer.sharedMaterials = materials;
        }
    }
}
