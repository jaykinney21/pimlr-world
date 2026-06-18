using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    public Dialogue ScreenDialogue;

    private void Awake ()
    {
        if(instance == null)
        {
            instance = this;
        }
    }


    public void StartDialogue (DialogueScriptable dialogue) 
    {
        ScreenDialogue.ShowDialogue(dialogue);
    }

    public void EndDialogue () 
    {
        ScreenDialogue.CloseDialogueBox();
    }
}
