using UnityEngine;
using UnityEngine.Events;


    [System.Serializable]
    public class Response
    {
        public string ResponseText;
        public DialogueScriptable DialogueScriptable;
        public VoidEvent ResponseEventVoid;
        public DialogueScriptableEvent ResponseEventDialogue;
        public DialogueScriptable DialogueScriptableEventPayload;
        public Vector3Event ResponseEventVector3;
        public Vector3 Vector3EventPayload;
        public bool isEvent;

    
}