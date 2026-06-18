using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    public class DialogueBoxcontroll : MonoBehaviour
    {
        [SerializeField] Dialogue dialogue;


        private void OnEnable()
        {
            dialogue.ShowDialogue();
        }

        public void closeDiague()
        {
            this.gameObject.SetActive(false);
        }


   
}
