using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDetector : MonoBehaviour
{
    [SerializeField]
    Canvas_Handler Canvas_Handler;

    bool gameover;

    private void OnTriggerEnter(Collider other)
    {
        if(gameover == true)
        {
            return;
        }
        string tag = other.gameObject.tag;
        
        Debug.Log("   winner >> "+tag);

        Canvas_Handler.RaceEnded(tag);
        gameover = true;

    }
}
