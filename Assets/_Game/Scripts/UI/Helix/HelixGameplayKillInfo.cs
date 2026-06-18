using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelixGameplayKillInfo : MonoBehaviour
{
    public RectTransform bg;

    public TMPro.TextMeshProUGUI deadPlayerUserName;
    public TMPro.TextMeshProUGUI killerPlayerUserName;

    public HelixGameplayKillInfo[] helixGameplayKillInfo = new HelixGameplayKillInfo[10];

    public HorizontalLayoutGroup horizontalLayoutGroup;
    public float autoDisableTime = 5f;
    void OnEnable()
    {

        transform.localScale = Vector3.one;
        // Invoke the method to disable the object after a certain time
        Invoke("DisablePopup", autoDisableTime);
        //horizontalLayoutGroup.enabled = false;
        //StartCoroutine(RebuildTransform());
    }

    //IEnumerator RebuildTransform()
    //{
    //    yield return new WaitForSeconds(0.1f);
    //    //horizontalLayoutGroup.enabled = true;
    //    LayoutRebuilder.ForceRebuildLayoutImmediate(bg);
    //}
    void OnDisable()
    {
        // Cancel any ongoing Invoke calls when the object is disabled
        CancelInvoke("DisablePopup");
    }
    void DisablePopup()
    {
        // Disable the object and return it to the object pool
        gameObject.SetActive(false);
        // Assuming there is an ObjectPool script attached to the same GameObject
        HelixKillInfoObjectPool objectPool = GetComponent<HelixKillInfoObjectPool>();
        if (objectPool != null)
        {
            objectPool.ReturnObjectToPool(this);
        }
    }
   

}
