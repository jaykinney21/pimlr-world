using System.Collections.Generic;
using UnityEngine;

public class PlmrDiractionFollowCharater : MonoBehaviour
{
    public List<GameObject> generatedMapObjects = new List<GameObject>();
    public Transform directionArrow;
    public float rotationSpeed;

    private void Update()
    {
        //FindDirection();
        point();
    }
   
    public void FindDirection()
    {
        GameObject nearestObject = FindNearestObject();
        if (nearestObject != null)
            directionArrow.rotation = Quaternion.Slerp(directionArrow.rotation,
            Quaternion.LookRotation(nearestObject.transform.position - directionArrow.position), rotationSpeed * Time.deltaTime);
    }
    public GameObject FindNearestObject()
    {
        GameObject nearestObject = null;
        float minDistance = float.MaxValue;
        foreach (GameObject obj in generatedMapObjects)
        {
            if (obj != null && obj.gameObject.activeSelf)
            {
                float distance = Vector3.Distance(directionArrow.position, obj.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestObject = obj.gameObject;
                }
            }
        }
        return nearestObject;
    }
    public void point()
    {
        GameObject nearestObject = FindNearestObject();
        if (nearestObject != null)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(nearestObject.transform.position);

            float yOffset = directionArrow.GetComponent<RectTransform>().sizeDelta.y/2;
            float xOffset = directionArrow.GetComponent<RectTransform>().sizeDelta.x/2;

            screenPos.x = Mathf.Clamp(screenPos.x, xOffset, Screen.width - xOffset);
            screenPos.y = Mathf.Clamp(screenPos.y, yOffset, Screen.height - yOffset);

            directionArrow.position = screenPos;
        }

    }


}
