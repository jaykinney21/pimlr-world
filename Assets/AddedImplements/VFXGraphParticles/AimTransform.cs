using JUTPS.FX;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimTransform : MonoBehaviour
{
    public Transform aimTransform;
    public HitMarkerEffect hitMarkerEffect;
    // Start is called before the first frame update
    void Start()
    {
       hitMarkerEffect = GetComponent<HitMarkerEffect>();
       aimTransform = hitMarkerEffect.transform;   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
