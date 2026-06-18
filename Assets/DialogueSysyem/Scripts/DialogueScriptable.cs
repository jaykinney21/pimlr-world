using UnityEngine;


    [CreateAssetMenu(menuName = "Dialogue/DialogueScriptable")]
    public class DialogueScriptable : ScriptableObject
    {
        [SerializeField] [TextArea] internal string[] dialogue;
        [SerializeField] private Response[] responses;

        public string[] Dialogue => dialogue;

        public bool HasResponses => Responses != null && Responses.Length > 0;

        public bool giveControlBackToManager;
        public Response[] Responses => responses;
    }

