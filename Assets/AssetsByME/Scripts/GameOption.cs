using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOption : MonoBehaviour {

	
	// Update is called once per frame
	void Update () {
        
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {

            Debug.Log("GameOption::>");
            if (Time.timeScale == 1)
            {
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1;
            }
        }
    }
}
