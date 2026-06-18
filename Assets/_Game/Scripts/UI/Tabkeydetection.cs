
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class Tabkeydetection : MonoBehaviour
{

    EventSystem system;


    #region UNITY_CALLBACKS
    void Start()
    {

        system = EventSystem.current;

    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Tab) && system.currentSelectedGameObject)
        {
            Selectable next = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift) ?
            system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnUp() :
            system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();
            if (next != null)
            {
                //Debug.Log(next.name);
                InputField inputfield = next.GetComponent<InputField>();
                if (inputfield != null)
                    inputfield.OnPointerClick(new PointerEventData(system));

                Dropdown dropdown = next.GetComponent<Dropdown>();
                if (dropdown != null)
                    dropdown.OnPointerClick(new PointerEventData(system));

                TMP_InputField OnTmpField = next.GetComponent<TMP_InputField>();
                if (OnTmpField != null)
                    OnTmpField.OnPointerClick(new PointerEventData(system));

                system.SetSelectedGameObject(next.gameObject);
            }
            else
            {
                Debug.Log("next Button null");
            }
            //Here is the navigating back part:
            //else
            //{
            //    next = Selectable.allSelectables[0];
            //    system.SetSelectedGameObject(next.gameObject);
            //}
        }

    }
    #endregion



}