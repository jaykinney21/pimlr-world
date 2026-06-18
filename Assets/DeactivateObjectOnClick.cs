using UnityEngine;
using UnityEngine.UI;

public class DeactivateObjectOnClick : MonoBehaviour
{
    public GameObject targetObject; // The object you want to deactivate

    private Button btn;

    private void Start()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(DeactivateTarget);
    }

    void DeactivateTarget()
    {
        if (targetObject != null)
        {
            targetObject.SetActive(false);
        }
    }
}
