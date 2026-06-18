using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace TS.Generics
{
    public class RadialMenu : MonoBehaviour
    {
        public Button buttonPrefab;
        public int numberOfButtons = 6;
        public float radius = 100f;
        //public List<Button> buttons = new List<Button>();
        public List<GameObject> buttons = new List<GameObject>();
        private void Start()
        {
            CreateRadialMenu();
        }
        #region Methode 1
        //[ContextMenu("CreateRadialMenu")]
        //private void CreateRadialMenu()
        //{

        //    float angleStep = 360f / numberOfButtons;
        //    for (int i = 0; i < numberOfButtons; i++)
        //    {
        //        float angle = i * angleStep;
        //        Vector2 position = CalculatePosition(angle);

        //        Button button = Instantiate(buttonPrefab, transform);
        //        button.transform.localPosition = position;
        //        // Rotate the button to face the center
        //        button.transform.rotation = Quaternion.Euler(0f, 0f, -angle);

        //        // Add text to the button
        //        //button.GetComponentInChildren<Text>().text = "Button " + (i + 1);

        //        // Add functionality to the button
        //        int buttonIndex = i; // Capture the current value of 'i'
        //        button.onClick.AddListener(() => OnButtonClick(buttonIndex));
        //    }
        //}

        //private Vector2 CalculatePosition(float angle)
        //{
        //    float angleInRadians = angle * Mathf.Deg2Rad;
        //    float x = Mathf.Cos(angleInRadians) * radius;
        //    float y = Mathf.Sin(angleInRadians) * radius;
        //    return new Vector2(x, y);
        //}

        //private void OnButtonClick(int buttonIndex)
        //{
        //    Debug.Log("Button " + (buttonIndex + 1) + " clicked!");
        //    // Add your button click functionality here
        //}
        #endregion

        [ContextMenu("CreateRadialMenu")]
        private void CreateRadialMenu()
        {
            float angleStep = 360f / numberOfButtons;
            for (int i = 0; i < buttons.Count; i++)
            {
                float angle = i * angleStep;
                Vector2 position = CalculatePosition(angle);

                //Button button = Instantiate(buttonPrefab, transform);
                buttons[i].transform.localPosition = position;
                buttons[i].gameObject.SetActive(true);

                // Rotate the button to face the center
                buttons[i].transform.rotation = Quaternion.Euler(0f, 0f, -angle);
               
            }
        }

        private Vector2 CalculatePosition(float angle)
        {
            float angleInRadians = angle * Mathf.Deg2Rad;
            float x = Mathf.Cos(angleInRadians) * radius;
            float y = Mathf.Sin(angleInRadians) * radius;
            return new Vector2(x, y);
        }
    }
}