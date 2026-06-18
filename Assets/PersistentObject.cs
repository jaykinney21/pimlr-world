using UnityEngine;

public class PersistentObject : MonoBehaviour
{
    private static bool isCharacterPresent = false;

    private void Awake()
    {
        if (isCharacterPresent)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        isCharacterPresent = true;
    }
}
