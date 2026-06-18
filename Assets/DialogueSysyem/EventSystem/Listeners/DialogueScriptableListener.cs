using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

    public class DialogueScriptableListener : BaseGameEventListener<DialogueScriptable, DialogueScriptableEvent, UnityDialogueScriptableEvent>
    {
        public void InitListener(UnityAction<DialogueScriptable> responseMethod, DialogueScriptableEvent thisEvent)
        {
            GameEvent = thisEvent;
            UnityEventResponse = new UnityDialogueScriptableEvent();
            UnityEventResponse.AddListener(responseMethod);

            if (GameEvent == null) { return; }

            GameEvent.RegisterListener(this);
        }
    }

