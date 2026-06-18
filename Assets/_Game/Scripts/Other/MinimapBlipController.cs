using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapBlipController : MonoBehaviour
{
    public Camera minimapCamera;             // Your top-down camera (player camera)
    public RectTransform minimapUI;          // The BlipLayer RectTransform
    public RectTransform playerCarBlip;         // Blue arrow blip
    public RectTransform policeBlip;         // Red dot blip
    public Transform playercar;                 // 3D player car
    public Transform policecar;                 // 3D police car
    public Transform player;                 // 3D player
    public RectTransform playerBlip;            //  player blid

    void Update()
    {
        if (playercar != null)
            UpdateBlipPosition(playercar, playerCarBlip);
        if (policecar != null)
            UpdateBlipPosition(policecar, policeBlip);

        // Optional: rotate player blip to match car's facing direction
        if (player != null)
            playerBlip.localRotation = Quaternion.Euler(0, 0, -playercar.eulerAngles.y);
    }

    void UpdateBlipPosition(Transform target, RectTransform blip)
    {
        Vector3 viewportPos = minimapCamera.WorldToViewportPoint(target.position);

        float x = (viewportPos.x - 0.5f) * minimapUI.rect.width;
        float y = (viewportPos.y - 0.5f) * minimapUI.rect.height;

        // Clamp to minimap bounds
        float halfWidth = 50;
        float halfHeight = 50;

        x = Mathf.Clamp(x, -halfWidth, halfWidth);
        y = Mathf.Clamp(y, -halfHeight, halfHeight);

        blip.anchoredPosition = new Vector2(x, y);

        // Optional: rotate blip with target
        //blip.localRotation = Quaternion.Euler(0, 0, -target.eulerAngles.y);
    }
}
