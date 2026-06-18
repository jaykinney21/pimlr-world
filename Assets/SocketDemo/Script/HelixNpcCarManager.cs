using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelixNpcCarManager : MonoBehaviour
{
    public static HelixNpcCarManager Instance;
    // Array of predefined names
    [Header("NPC Name")]
    private string[] names = { "Alice", "Bob", "Charlie", "David", "Eva", "Frank", "Grace", "Henry",
                           "Isabel", "Jack", "Katherine", "Liam", "Mia", "Nathan", "Olivia", "Peter",
                           "Quinn", "Rachel", "Samuel", "Tessa", "Ulysses", "Victoria", "Walter", "Xena",
                           "Yasmine", "Zane" };
    // List to keep track of used names
    private List<string> usedNames = new List<string>();
    public string randomName;

    string teamSelect;
    // Example of how to use the random name generator
    private void Start()
    {
        Instance = this;      
    }


    #region GENERATE-RANDOM-NAME
    // Function to generate a random name
    internal string GenerateRandomName()
    {
        // If all names have been used, reset the list
        if (usedNames.Count == names.Length)
        {
            usedNames.Clear();
        }

        // Get a random index from the array that hasn't been used
        int randomIndex;
        do
        {
            randomIndex = Random.Range(0, names.Length);
        } while (usedNames.Contains(names[randomIndex]));

        // Mark the name as used
        usedNames.Add(names[randomIndex]);

        // Return the randomly selected name
        return names[randomIndex];
    }

    #endregion

   
}
