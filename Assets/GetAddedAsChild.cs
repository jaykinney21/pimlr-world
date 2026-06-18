using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetAddedAsChild : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        // Find the AdvancedCarSystem GameObject in the scene
        GameObject advancedCarSystem = GameObject.Find("AdvancedCarSystem");

        // If found, set the current GameObject as its child
        if (advancedCarSystem != null)
        {
            transform.SetParent(advancedCarSystem.transform);
        }
        else
        {
            Debug.LogWarning("AdvancedCarSystem GameObject not found in the scene!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
