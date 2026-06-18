using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] objectPrefabs; // List of different object prefabs
    public int numberOfObjects = 10;
    public float spawnRadius = 5f;

    /* [ContextMenu("Setup Spawn Points")]
     void SetupSpawnPoints()
     {
         if (objectPrefabs == null || objectPrefabs.Length == 0)
         {
             Debug.LogError("No object prefabs assigned. Please assign prefabs in the inspector.");
             return;
         }

         for (int i = 0; i < numberOfObjects; i++)
         {
             float angle = i * (360f / numberOfObjects);
             float x = spawnRadius * Mathf.Cos(Mathf.Deg2Rad * angle);
             float z = spawnRadius * Mathf.Sin(Mathf.Deg2Rad * angle);

             Vector3 spawnPosition = new Vector3(x, 0f, z);

             // Randomly select an object prefab from the list
             GameObject prefabToSpawn = objectPrefabs[Random.Range(0, objectPrefabs.Length)];

             // Create an empty GameObject at the specified position
             GameObject spawnPoint = new GameObject("SpawnPoint_" + i);
             spawnPoint.transform.position = spawnPosition;
             spawnPoint.transform.rotation = Quaternion.identity;
             // Attach the selected prefab as a child of the spawn point
             GameObject spawnedObject = Instantiate(prefabToSpawn, spawnPoint.transform);
             spawnedObject.name = prefabToSpawn.name; // Optionally set the name of the spawned object
         }
     }*/

    [ContextMenu("Setup Spawn Points")]
    void SetupSpawnPoints()
    {
        if (objectPrefabs == null || objectPrefabs.Length == 0)
        {
            Debug.LogError("No object prefabs assigned. Please assign prefabs in the inspector.");
            return;
        }

        for (int i = 0; i < Mathf.Min(numberOfObjects, objectPrefabs.Length); i++)
        {
            float angle = i * (360f / numberOfObjects);
            float x = spawnRadius * Mathf.Cos(Mathf.Deg2Rad * angle);
            float z = spawnRadius * Mathf.Sin(Mathf.Deg2Rad * angle);

            Vector3 spawnPosition = new Vector3(x, 0f, z);

            // Get the object from the list based on index
            GameObject objectToTransform = objectPrefabs[i];

            // Check if the objectToTransform is null
            if (objectToTransform != null)
            {
                // Set the position and rotation directly on the existing object
                objectToTransform.transform.position = spawnPosition;
                objectToTransform.transform.rotation = Quaternion.identity;
            }
            else
            {
                Debug.LogWarning("Object not found for index " + i);
            }
        }
    }

   


}



