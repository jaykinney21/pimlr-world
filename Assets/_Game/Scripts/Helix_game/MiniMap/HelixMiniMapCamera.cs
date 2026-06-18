// PIMLR #1: removed unused 'using CarControllerwithShooting;' (Helix separation).
// No type from that namespace is referenced in this file, so the directive was dead.
// (HelixHealthBar.cs intentionally KEEPS the using — it references CarController.)
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelixMiniMapCamera : MonoBehaviour
{
    #region Map 1
    public Transform player;  // Reference to the player's transform
    public float mapScale = 0.1f;  // Adjust the scale of the minimap
    //public RectTransform playerIndicator;
    void LateUpdate()
    {
        //RectTransform rectTransform = GetComponent<RectTransform>();

        this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, 45f, this.gameObject.transform.position.z);
        // Update the position of the minimap camera based on the player's position
        Vector3 newPosition = player.position;
        newPosition.y = transform.position.y;  // Keep the same y-coordinate
        transform.position = newPosition;

        // Update the rotation of the minimap camera based on the player's rotation
        transform.rotation = Quaternion.Euler(90f, player.eulerAngles.y, 0f);

        // Update the minimap UI Raw Image position based on the player's position
        Vector2 mapPos = new Vector2(player.position.x, player.position.z) * mapScale;
        mapPos += new Vector2(0.5f, 0.5f);  // Center the minimap on the screen
        /* if (rectTransform != null)
         {
             rectTransform.anchoredPosition = mapPos;
             //GetComponent<RectTransform>().anchoredPosition = mapPos;
         }
         else
         {
             Debug.LogError("RectTransform component not found on the GameObject.");
         }*/
        //GetComponent<RectTransform>().anchoredPosition = mapPos;

        //Update the player indicator position on the minimap
        Vector2 playerPos = new Vector2(player.position.x, player.position.z) * mapScale;
        //playerIndicator.anchoredPosition = playerPos;
    }
    #endregion

    #region Map 2
    //public Transform player;  // Reference to the player's transform
    //public float mapScale = 0.1f;  // Adjust the scale of the minimap
    //public RectTransform playerIndicator;  // Reference to the UI element representing the player on the minimap
    ////public RectTransform directionIndicator;  // Reference to the UI element representing the player's facing direction
    //void LateUpdate()
    //{
    //    // Update the position of the minimap camera based on the player's position
    //    Vector3 newPosition = player.position;
    //    newPosition.y = transform.position.y;  // Keep the same y-coordinate
    //    transform.position = newPosition;

    //    // Update the rotation of the minimap camera based on the player's rotation
    //    transform.rotation = Quaternion.Euler(90f, player.eulerAngles.y, 0f);

    //    // Update the minimap UI Raw Image position based on the player's position
    //    Vector2 mapPos = new Vector2(player.position.x, player.position.z) * mapScale;
    //    mapPos += new Vector2(0.5f, 0.5f);  // Center the minimap on the screen
    //    GetComponent<RectTransform>().anchoredPosition = mapPos;

    //    // Update the player indicator position on the minimap
    //    Vector2 playerPos = new Vector2(player.position.x, player.position.z) * mapScale;
    //    playerIndicator.anchoredPosition = playerPos;

    //    // Update the compass text based on the player's rotation
    //    //UpdateCompassText();
    //    // Update the direction indicator rotation based on the player's facing direction
    //    //GameCanvas.Instance.directionIndicator.rotation = Quaternion.Euler(0f, 0f, -player.eulerAngles.y);
    //}
    #endregion

    #region Map 3
    //public Transform player;  // Reference to the player's transform
    //public float mapScale = 0.1f;  // Adjust the scale of the minimap
    ////public RectTransform playerIndicator;  // Reference to the UI element representing the player on the minimap
    ////public TextMeshProUGUI compassText;  // Reference to the UI Text element representing the compass

    //void LateUpdate()
    //{
    //    // Update the position of the minimap camera based on the player's position
    //    Vector3 newPosition = player.position;
    //    newPosition.y = transform.position.y;  // Keep the same y-coordinate
    //    transform.position = newPosition;

    //    // Update the rotation of the minimap camera based on the player's rotation
    //    transform.rotation = Quaternion.Euler(90f, player.eulerAngles.y, 0f);

    //    // Update the minimap UI Raw Image position based on the player's position
    //    Vector2 mapPos = new Vector2(player.position.x, player.position.z) * mapScale;
    //    mapPos += new Vector2(0.5f, 0.5f);  // Center the minimap on the screen
    //    GetComponent<RectTransform>().anchoredPosition = mapPos;

    //    // Update the player indicator position on the minimap
    //    Vector2 playerPos = new Vector2(player.position.x, player.position.z) * mapScale;
    //    GameCanvas.Instance.playerIndicator.anchoredPosition = playerPos;

    //    // Update the compass text based on the player's rotation
    //    UpdateCompassText();
    //}

    //void UpdateCompassText()
    //{
    //    // Calculate the cardinal direction based on the player's rotation
    //    float angle = player.eulerAngles.y;
    //    string direction = GetCardinalDirection(angle);

    //    // Update the compass text
    //    GameCanvas.Instance.compassText.text = direction;
    //}

    //string GetCardinalDirection(float angle)
    //{
    //    // Convert the angle to a cardinal direction (North, East, South, West)
    //    string[] directions = { "N", "NE", "E", "SE", "S", "SW", "W", "NW", "N" };
    //    int index = Mathf.RoundToInt(angle / 45f) % 8;
    //    return directions[index];
    //}
    #endregion
}


