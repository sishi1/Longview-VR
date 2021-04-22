using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Valve.VR.InteractionSystem
{
    public class ButtonHints : MonoBehaviour
    {
        private Player player = null;

        [Header("Input buttons")]
        public SteamVR_Action_Boolean notebookButton;
        public SteamVR_Action_Boolean openDoorButton;

        [Header("Hints info")]
        [SerializeField]
        private string hintObject;
        [SerializeField]
        private string hintText;

        private void Start()
        {
            player = Player.instance;
        }

        private void Update()
        {
            foreach (Hand hand in player.hands)
            {
                if (StaticVariables.doorUsed)
                    ControllerButtonHints.HideTextHint(hand, openDoorButton);

                if (StaticVariables.noteBookUsed)
                    ControllerButtonHints.HideTextHint(hand, notebookButton);

            }
        }

        private void DoorHint()
        {
            //Show the hint on each eligible hand
            foreach (Hand hand in player.hands)
                ControllerButtonHints.ShowTextHint(hand, openDoorButton, hintText);
        }

        private void NotebookHint()
        {
            foreach (Hand hand in player.hands)
                ControllerButtonHints.ShowTextHint(hand, notebookButton, hintText);
        }


        //private IEnumerator NotebookHint(ISteamVR_Action_In_Source action, string text)
        //{
        //    while (true)
        //    {
        //        //Show the hint on each eligible hand
        //        foreach (Hand hand in player.hands)
        //        {
        //            ControllerButtonHints.ShowTextHint(hand, action, text);

        //            if (Notebook.used)
        //            {
        //                ControllerButtonHints.HideTextHint(hand, action);
        //            }
        //        }

        //        yield return null;
        //    }
        //}

        private void OnTriggerEnter(Collider other)
        {
            switch(hintObject)
            {
                case "Door":
                    DoorHint();
                    break;

                case "Notebook":
                    NotebookHint();
                    break;

                case null:
                    Debug.LogErrorFormat("Something went wrong in the switch statement", hintObject);
                    break;
            }
        }
    }
}
