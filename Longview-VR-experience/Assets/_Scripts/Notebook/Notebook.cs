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
                if (!notebookCanvas.enabled)
                {
                    notebookCanvas.enabled = true;

                    textComponent.text = "<#000000>" + "Interessant:" + "</color>" + "\n";
                    foreach (string markedObject in markedObjectsForInterest)
                    {
                        textComponent.text += "- " + markedObject + "\n";
                    }

                    textComponent.text += "\n" + "<#000000>" + "In beslag nemen:" + "</color>" + "\n";
                    foreach (string markedObject in markedObjectsForConfiscate)
                    {
                        textComponent.text += "- " + markedObject + "\n";
                    }

                    textComponent.text += "\n" + "<#000000>" + "Specialist:" + "</color>" + "\n";
                    foreach (string markedObject in markedObjectsForSpecialist)
                    {
                        textComponent.text += "- " + markedObject + "\n";
                    }
                }
                else
                {
                    notebookCanvas.enabled = false;

                    if (!TutorialManager.hasExplainedNotebook)
                    {
                        TutorialManager.isExplainingNotebook = false;
                        TutorialManager.hasExplainedNotebook = true;
                        TutorialManager.endTutorial = true;
                    }
                }
            }
        }
    }
}

