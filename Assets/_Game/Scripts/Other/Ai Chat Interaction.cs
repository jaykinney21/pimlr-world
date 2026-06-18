using JUTPS;
using JUTPS.JUInputSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiChatInteraction : MonoBehaviour
{

    public GameObject ai_BOT_Canvas;
    [SerializeField] internal JUCharacterController characterController; //AUTO ASSIGN
    private void Start()
    {
        ai_BOT_Canvas.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ai_BOT_Canvas.SetActive(true);
            characterController = other.gameObject.GetComponent<JUCharacterController>();

            PlayerControllerStop();
        }
    }
    void PlayerControllerStop()
    {
        characterController.BlockHorizontalInput = true;

        characterController.BlockVerticalInput = true;
        characterController.BlockFireModeOnCursorVisible = true;
        characterController.CanMove = false;
        characterController.CanJump = false;
        characterController.CanRotate = false;
        characterController.EnableRoll = false;
        characterController.UseDefaultControllerInput = false;



        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    [System.Obsolete]
    public void OnClosePanel()
    {
        SceneManagerScript.Instance.musicSystem.musicSystem.volume = 0.1f;
        ai_BOT_Canvas.SetActive(false);

        Application.ExternalCall("stopSpeaking");
        PlayerControllerStart();


    }
    public void PlayerControllerStart()
    {
        characterController.BlockHorizontalInput = false;

        characterController.BlockVerticalInput = false;

        characterController.BlockFireModeOnCursorVisible = false;
        characterController.CanMove = true;
        characterController.CanJump = true;
        characterController.CanRotate = true;
        characterController.EnableRoll = true;
        characterController.UseDefaultControllerInput = true;
        //characterController.enabled = true;
        //JUInput.Instance().EnableBlockStandardInputs();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    

}
