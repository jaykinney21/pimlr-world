using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderValueAssignerLoadingScreen : MonoBehaviour
{
    public Slider loadingSlider;

    public Text loadingText;

    private void OnEnable () 
    {
        loadingSlider.value = 0;

        loadingSlider.DOValue(1, 4f);
    }
    float percentage;
    // Update is called once per frame
    void Update()
    {
        //if (loadingSlider.value.ToString().Length > 3)
        //{
        //    loadingText.text = loadingSlider.value.ToString().Length > 5 ? (loadingSlider.value * 100).ToString().Substring(0, 2) + '%' : (loadingSlider.value * 100).ToString() + ".00%";
        //}

        float percentage = loadingSlider.value * 100f;

        // Update the percentage display with two decimal places
        loadingText.text = percentage.ToString("F2") + "%";
    }
}
