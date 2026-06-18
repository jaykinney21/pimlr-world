using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customization_Applier : changeColor
{


    #region UnityCallbacks
    private void Start()
    {
        GetPlayPrefs();
        ApplyCustomization();
    }
    #endregion


    void GetPlayPrefs()
    {
        if (renderers != null)
        {
            for (int i = 0; i < renderers.Length; i++)
            {
                if (renderers[i] != null)
                {
                    string rendererName = renderers[i].name;
                    PlayerPrefs.GetInt(rendererName, 0);
                }
            }
        }
    }


    void ApplyCustomization()
    {
        if (renderers != null)
        {
            for (int i = 0; i < renderers.Length; i++)
            {
                string rendererName = renderers[i].name;
                renderers[i].materials[0].color = colors[PlayerPrefs.GetInt(rendererName, 0)];
                colourableMaterials[i].color = colors[PlayerPrefs.GetInt(rendererName, 0)];
            }
        }
    }
}
