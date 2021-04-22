using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Valve.VR
{
    public class Notebook : MonoBehaviour
    {
        [Header("SteamVR Input")]
        public SteamVR_Input_Sources rightHand;
        public SteamVR_Action_Boolean bButton;

        [Header("Marked Objects")]
        public List<string> markedObjectsForInterest = new List<string>();
        public List<string> markedObjectsForConfiscate = new List<string>();
        public List<string> markedObjectsForSpecialist = new List<string>();

        [Header("UI")]
        public GameObject notebook;
        private Canvas notebookCanvas;
        public GameObject textObject;
        private TextMeshProUGUI textComponent;

        void Start()
        {
            textComponent = textObject.GetComponent<TextMeshProUGUI>();
            notebookCanvas = notebook.GetComponent<Canvas>();
        }

        void Update()
        {
            if (bButton.GetStateDown(rightHand))
            {
                StaticVariables.noteBookUsed = true;

                if (!notebookCanvas.enabled)
                {
                    notebookCanvas.enabled = true;

                    textComponent.text = "<#FFFF00>" + "Interessant:" + "</color>" + "\n";
                    foreach (string markedObject in markedObjectsForInterest)
                    {
                        textComponent.text += "- " + markedObject + "\n";
                    }

                    textComponent.text += "\n" + "<#00FF00>" + "In beslag nemen:" + "</color>" + "\n";
                    foreach (string markedObject in markedObjectsForConfiscate)
                    {
                        textComponent.text += "- " + markedObject + "\n";
                    }

                    textComponent.text += "\n" + "<#00BEFF>" + "Specialist:" + "</color>" + "\n";
                    foreach (string markedObject in markedObjectsForSpecialist)
                    {
                        textComponent.text += "- " + markedObject + "\n";
                    }
                }
                else
                    notebookCanvas.enabled = false;
            }
        }
    }
}

