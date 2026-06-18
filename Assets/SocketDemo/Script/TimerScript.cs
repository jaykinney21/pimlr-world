using CarControllerwithShooting;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerScript : MonoBehaviour
{
    public float startTime = 20f; // Initial time in seconds
    public float countdownTime; // Remaining time in seconds
    public float t;
    public string timeString;

    private void Start()
    {
        countdownTime = startTime;
    }

    private void Update()
    {
        //TimerUpdate();
    }

    public void TimerUpdate()
    {
        // Update the countdown time
        countdownTime -= Time.deltaTime;

        // Convert the remaining time to minutes and seconds
        int minutes = Mathf.FloorToInt(countdownTime / 60);
        int seconds = Mathf.FloorToInt(countdownTime % 60);

        // Display the time in a specific format (e.g., "00:00")
        string timeString = string.Format("{0:00}:{1:00}", minutes, seconds);
        GameCanvas.Instance.text_timer.text = timeString;
        // Output the time to the console (you can use this value in your game as needed)


        t += Time.deltaTime;
        if (t > 1)
        {
            t -= 1;
            /* if (SocketDemoManager.Instance.PlayerManager.IsMaster)*/
            //SocketNetworkManager.Instance.emitSetTimer(timeString);
        }
        Debug.Log(timeString);

        // Check if the countdown has reached zero
        if (countdownTime <= 0f)
        {
            // Do something when the timer reaches zero (e.g., end the game)
            Debug.Log("Timer reached zero!");

        }
    }

}

