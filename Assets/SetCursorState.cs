using UnityEngine;

public class SetCursorState : MonoBehaviour
{
    private void Start()
    {
        Cursor.lockState = CursorLockMode.None; // Unlock the cursor
        Cursor.visible = true;                  // Make the cursor visible
    }
}
