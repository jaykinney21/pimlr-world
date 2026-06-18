using UnityEngine;

public class ToggleObjects : MonoBehaviour
{
    public GameObject[] objectsToToggle;
    public GameObject thurz;

    public void ToggleActiveState()
    {
        Vector3 rotation = thurz.transform.eulerAngles;
        rotation.y = 0;
        thurz.transform.eulerAngles = rotation;

        foreach (GameObject obj in objectsToToggle)
        {
            obj.SetActive(!obj.activeSelf);
        }
    }
}
