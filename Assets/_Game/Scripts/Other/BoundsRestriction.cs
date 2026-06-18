using UnityEngine;

// PIMLR #5: Movement leash until the vehicle is unlocked.
// Clamps the player's transform to a serialized axis-aligned bounding box (AABB)
// every LateUpdate while GameExecutionManager.vehicleUnlocked == false.
// Once the vehicle is unlocked the restriction disables itself (and stops clamping),
// letting the player roam the full block/downtown.
//
// EDITOR WIRING (residual step):
//   1. Create an empty GameObject in YannicksWorld.unity, e.g. "MovementLeash",
//      add this component.
//   2. Set Center/Size to cover the ~half-block starting area (gizmo draws the box
//      in the Scene view so you can size it visually). Center is WORLD space.
//   3. Optionally assign 'player' explicitly; otherwise it auto-resolves from
//      GameExecutionManager.Instance.playerHandler.jUCharacterController at runtime.
[AddComponentMenu("PIMLR/Bounds Restriction (Movement Leash)")]
public class BoundsRestriction : MonoBehaviour
{
    [Header("Leash Bounds (world-space AABB)")]
    [Tooltip("World-space center of the allowed area while the vehicle is locked.")]
    [SerializeField] private Vector3 center = Vector3.zero;

    [Tooltip("Full size (width/height/depth) of the allowed area.")]
    [SerializeField] private Vector3 size = new Vector3(40f, 50f, 40f);

    [Header("References")]
    [Tooltip("Player transform to clamp. Leave null to auto-resolve from GameExecutionManager.")]
    [SerializeField] private Transform player; // AUTO-RESOLVED if null

    [Header("Options")]
    [Tooltip("If true, also clamp the Y axis. Usually leave off so jumps/falls aren't fought.")]
    [SerializeField] private bool clampVertical = false;

    private bool _resolvedPlayer;

    private void OnEnable()
    {
        // Disable immediately if the vehicle is already unlocked (e.g. returning to the scene).
        if (GameExecutionManager.Instance != null)
        {
            GameExecutionManager.Instance.onVehicleUnlocked += HandleVehicleUnlocked;
            if (GameExecutionManager.Instance.vehicleUnlocked)
            {
                enabled = false;
            }
        }
    }

    private void OnDisable()
    {
        if (GameExecutionManager.Instance != null)
        {
            GameExecutionManager.Instance.onVehicleUnlocked -= HandleVehicleUnlocked;
        }
    }

    private void HandleVehicleUnlocked()
    {
        // PIMLR #5: leash released - stop clamping.
        enabled = false;
    }

    private void TryResolvePlayer()
    {
        if (player != null) { _resolvedPlayer = true; return; }

        var gem = GameExecutionManager.Instance;
        if (gem != null && gem.playerHandler != null && gem.playerHandler.jUCharacterController != null)
        {
            player = gem.playerHandler.jUCharacterController.transform;
            _resolvedPlayer = player != null;
        }
    }

    private void LateUpdate()
    {
        // Safety: if something else flipped the flag, bail out.
        if (GameExecutionManager.Instance != null && GameExecutionManager.Instance.vehicleUnlocked)
        {
            enabled = false;
            return;
        }

        if (!_resolvedPlayer) TryResolvePlayer();
        if (player == null) return;

        Vector3 half = size * 0.5f;
        Vector3 min = center - half;
        Vector3 max = center + half;

        Vector3 p = player.position;
        p.x = Mathf.Clamp(p.x, min.x, max.x);
        p.z = Mathf.Clamp(p.z, min.z, max.z);
        if (clampVertical) p.y = Mathf.Clamp(p.y, min.y, max.y);

        player.position = p;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0.2f, 0.8f, 1f, 0.25f);
        Gizmos.DrawCube(center, size);
        Gizmos.color = new Color(0.2f, 0.8f, 1f, 1f);
        Gizmos.DrawWireCube(center, size);
    }
}
