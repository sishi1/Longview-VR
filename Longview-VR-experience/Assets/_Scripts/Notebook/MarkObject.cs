using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Valve.VR.InteractionSystem
{
    public class MarkObject : MonoBehaviour
    {
        private Player player = null;

        [Header("Miscellaneous")]
        public Transform controller;

        private GameObject selectedObject;
        public GameObject markObjectUI;
        private Canvas markObjectCanvas;

        [Header("SteamVR Input")]
        public SteamVR_Input_Sources rightHand;
        public SteamVR_Action_Boolean aButton;
        public SteamVR_Action_Vector2 joystickSelection;
        public SteamVR_Action_Boolean trigger;

        [Header("Hints")]
        [SerializeField]
        private string openSelectionMenuHint;
        [SerializeField]
        private string joystickSelectionHint;


        [HideInInspector]
        public ObjectState objectState;
        [HideInInspector]
        public string selection;

        private int childCount;
        private int maxChildCount;

        private bool usedAButton;
        private bool usedJoystick;

        private void Start()
        {
            player = Player.instance;
            markObjectCanvas = markObjectUI.GetComponent<Canvas>();
            childCount = Player.instance.rightHand.gameObject.transform.childCount + 0;
            maxChildCount = 11;
        }

        private void Update()
        {
            //Checks whether the player has an object in hand
            if (player.rightHand.gameObject.transform.childCount != childCount)
            {
                //Attach the object in hand in the variable
                selectedObject = player.rightHand.transform.GetChild(player.rightHand.transform.childCount - 1).gameObject;
                objectState = selectedObject.GetComponent<ObjectState>();
                
            }

            if (player.rightHand.transform.childCount == maxChildCount && !usedAButton)
                OpenHint(aButton, openSelectionMenuHint);

            //Two buttons can't be true at the same time, hence why trigger state is false since that's the first button that was activated and becomes false
            if (aButton.GetStateDown(rightHand) && !trigger.GetStateDown(rightHand))
                if (player.rightHand.transform.childCount == maxChildCount)
                {
                    usedAButton = true;
                    markObjectCanvas.enabled = true;
                }

            if (markObjectCanvas.enabled)
            {
                HideHint(aButton);

                if (!usedJoystick)
                    OpenHint(joystickSelection, joystickSelectionHint);

                JoystickSelection();
            }

            //Reset everything when you stop holding an object in hand
            if (!aButton.GetStateDown(rightHand) && !trigger.GetStateDown(rightHand))
                if (player.rightHand.transform.childCount != maxChildCount)
                    ResetStatus();
        }

        private void JoystickSelection()
        {
            //left/interesting
            if (joystickSelection.axis.x >= -1f && joystickSelection.axis.x < -0.2f && joystickSelection.axis.y >= -0.71f && joystickSelection.axis.y < 0.71f)
            {
                selection = "interesting";
                usedJoystick = true;
            }
            //top/confiscate
            else if (joystickSelection.axis.x >= -0.71f && joystickSelection.axis.x <= 0.71f && joystickSelection.axis.y > 0.2f && joystickSelection.axis.y <= 1f)
            {
                selection = "confiscate";
                usedJoystick = true;
            }
            //right/ask specialist
            else if (joystickSelection.axis.x <= 1f && joystickSelection.axis.x > 0.2f && joystickSelection.axis.y >= -0.71f && joystickSelection.axis.y < 0.71f)
            {
                selection = "specialist";
                usedJoystick = true;
            }
            //bottom/nothing
            else if (joystickSelection.axis.x <= 0.71f && joystickSelection.axis.x > -0.71f && joystickSelection.axis.y >= -1f && joystickSelection.axis.y < -0.2f)
            {
                selection = "nothing";
                usedJoystick = true;
            }

            ConfirmSelection();
        }

        private void ConfirmSelection()
        {
            if (usedJoystick)
                HideHint(joystickSelection);

            if (joystickSelection.axis.x == 0 && joystickSelection.axis.y == 0)
            {
                switch (selection)
                {
                    case "interesting":
                        objectState.MarkForInterest();
                        ResetStatus();
                        markObjectCanvas.enabled = false;
                        break;

                    case "confiscate":
                        objectState.MarkForConfiscate();
                        ResetStatus();
                        markObjectCanvas.enabled = false;
                        break;

                    case "specialist":
                        objectState.MarkForSpecialist();
                        ResetStatus();
                        markObjectCanvas.enabled = false;
                        break;

                    case "nothing":
                        objectState.MarkForNothing();
                        ResetStatus();
                        markObjectCanvas.enabled = false;
                        break;

                    case null:
                        Debug.LogErrorFormat("Something went wrong in the switch statement", selection);
                        break;
                }
            }
        }

        private void OpenHint(ISteamVR_Action_In_Source action, string text)
        {
            foreach (Hand hand in player.hands)
                ControllerButtonHints.ShowTextHint(hand, action, text);
        }

        private void HideHint(ISteamVR_Action_In_Source action)
        {
            foreach (Hand hand in player.hands)
                ControllerButtonHints.HideTextHint(hand, action);
        }

        private void ResetStatus()
        {
            selection = "";
            markObjectCanvas.enabled = false;
        }
    }
}


