using JUTPS;
using UnityEngine;
using UnityEngine.UI;

// PIMLR #11: Stamina bar UI.
// JUTPS already implements a sprint taper internally (SprintSpeedDecrease in
// JUCharacterControllerCore: ramps 0 -> MaxSprintSpeedDecrease while sprinting,
// then decays back to 0). The meter just wasn't shown. This drives an Image.fillAmount
// from the player's NormalizedSprintStamina (exposed read-only on the core).
//
// EDITOR WIRING (residual step):
//   1. Create a UI Image (Image Type = Filled) for the stamina bar on the HUD Canvas
//      (e.g. inside the existing "JUTPS Interface" menu).
//   2. Add this component anywhere on that Canvas and assign 'fillImage' to that Image.
//   3. Leave 'player' null to auto-resolve from GameExecutionManager at runtime, or
//      assign the player's JUCharacterController explicitly.
[AddComponentMenu("PIMLR/UI Stamina Bar")]
public class UIStaminaBar : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Filled Image whose fillAmount represents remaining sprint stamina.")]
    [SerializeField] private Image fillImage; // ASSIGN IN EDITOR

    [Tooltip("Player controller. Leave null to auto-resolve from GameExecutionManager.")]
    [SerializeField] private JUCharacterController player; // AUTO-RESOLVED if null

    [Header("Options")]
    [Tooltip("Smoothing speed for the bar. 0 = snap instantly.")]
    [SerializeField] private float lerpSpeed = 8f;

    [Tooltip("Hide the bar entirely when stamina is full (idle).")]
    [SerializeField] private bool hideWhenFull = false;

    [Tooltip("Tint applied while stamina is depleted (can't sprint).")]
    [SerializeField] private Color depletedColor = new Color(1f, 0.4f, 0.4f, 1f);
    [SerializeField] private Color normalColor = Color.white;

    private void TryResolvePlayer()
    {
        if (player != null) return;

        var gem = GameExecutionManager.Instance;
        if (gem != null && gem.playerHandler != null)
        {
            player = gem.playerHandler.jUCharacterController;
        }
    }

    private void Update()
    {
        if (fillImage == null) return;

        if (player == null)
        {
            TryResolvePlayer();
            if (player == null) return;
        }

        float target = player.NormalizedSprintStamina;

        if (lerpSpeed <= 0f)
        {
            fillImage.fillAmount = target;
        }
        else
        {
            fillImage.fillAmount = Mathf.Lerp(fillImage.fillAmount, target, lerpSpeed * Time.deltaTime);
        }

        // Colour feedback: red-ish when the player has run out of sprint.
        fillImage.color = player.CanSprintNow ? normalColor : depletedColor;

        if (hideWhenFull)
        {
            bool show = target < 0.999f;
            if (fillImage.enabled != show) fillImage.enabled = show;
        }
    }
}
