using UnityEngine;

[RequireComponent(typeof(Transform))]
public class AddMeshColliders : MonoBehaviour
{
    // Add a context menu option to the Transform component in the Unity inspector
    [ContextMenu("Add Mesh Colliders To Children")]
    void AddMeshCollidersToChildren()
    {
        // Get all the MeshRenderers in this object and its children
        MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>();

        // Loop through each MeshRenderer and add a MeshCollider if one doesn't exist
        foreach (MeshRenderer renderer in renderers)
        {
            if (renderer.gameObject.GetComponent<MeshCollider>() == null)
            {
                renderer.gameObject.AddComponent<MeshCollider>();
            }
        }
    }
}
