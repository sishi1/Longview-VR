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

        [SerializeField] private LineRenderer line;
        [SerializeField] private LayerMask checkLayer;

        private GameObject selectedObject;

        [SerializeField] private GameObject steamVRTeleport;
        [SerializeField] private GameObject snapTurn;

        [Header("SteamVR Input")]
        public SteamVR_Input_Sources hands;
        public SteamVR_Action_Boolean aButton;
        public SteamVR_Action_Vector2 joystickSelection;
        public SteamVR_Action_Boolean trigger;
        public SteamVR_Action_Boolean grip;

        [Header("Hints")]
        [SerializeField]
        private string openSelectionMenuHint;
        [SerializeField]
        private string joystickSelectionHint;

        [HideInInspector]
        public ObjectState objectState;
        [HideInInspector]
        public string selection;

        private int childCountRightHand;
        private int childCountLeftHand;
        private int maxChildCountRightHand;
        private int maxChildCountLeftHand;

        private bool usedAButton;
        private bool usedJoystick;
        private bool objectInHand = false;
        private bool objectHit = false;
        private bool activateGaze = false;

        private float timer = 0f;
        private int seconds = 0;
        private int activationTime = 5;

        private void Start()
        {
            player = Player.instance;

            childCountRightHand = player.rightHand.gameObject.transform.childCount + 0;
            childCountLeftHand = player.leftHand.gameObject.transform.childCount + 0;

            maxChildCountRightHand = 10;
            maxChildCountLeftHand = 7;
        }

        private void Update()
        {
            if (grip.GetStateDown(hands))
            {
                if (activateGaze)
                    activateGaze = false;
                else
                    activateGaze = true;
            }

            if (activateGaze)
            {
                line.enabled = true;

                if (Physics.Raycast(controller.position, controller.forward, out RaycastHit hit, StaticVariables.lineLength, checkLayer))
                {
                    Debug.Log(timer);
                    timer += Time.deltaTime;

                    if (timer >= 1f)
                    {
                        seconds++;
                        timer = 0;
                    }

                    if (seconds >= activationTime)
                    {
                        selectedObject = hit.collider.gameObject;
                        objectState = selectedObject.GetComponent<ObjectState>();
                        objectHit = objectState != null;

                        if (objectHit)
                        {
                            StaticVariables.activateMenuSelection = true;
                            JoystickSelection();

                            StaticVariables.joystickMovementActive = false;
                            steamVRTeleport.SetActive(false);
                            snapTurn.SetActive(false);
                        }
                    }
                }
                else
                    ResetStatus();
            }
            else
            {
                line.enabled = false;

                if (selectedObject != null)
                    Debug.Log($"<b>markobject </b> Object state: {(objectState == null ? "null" : objectState.ToString())} Selected object: {selectedObject.name}");

                Debug.Log($"<b>markobject </b> Right standard child count: {childCountRightHand} Right child count: {player.rightHand.gameObject.transform.childCount}");
                Debug.Log($"<b>markobject </b> Left standard child count: {childCountLeftHand} Left child count: {player.leftHand.gameObject.transform.childCount}");

                //Debug.Log($"<b>MarkObject </b> x-as: {joystickSelection.axis.x} y-as: {joystickSelection.axis.y}");

                //Checks whether the player has an object in hand
                if (player.rightHand.gameObject.transform.childCount != childCountRightHand && !objectInHand)
                {
                    //Attach the object in hand in the variable
                    selectedObject = player.rightHand.transform.GetChild(player.rightHand.transform.childCount - 1).gameObject;
                    objectState = selectedObject.GetComponent<ObjectState>();
                    objectInHand = objectState != null;

                    if (objectInHand)
                    {
                        StaticVariables.joystickMovementActive = false;
                        steamVRTeleport.SetActive(false);
                        snapTurn.SetActive(false);
                    }
                }

                if (player.leftHand.gameObject.transform.childCount != childCountLeftHand && !objectInHand)
                {
                    //Attach the object in hand in the variable
                    selectedObject = player.leftHand.transform.GetChild(player.leftHand.transform.childCount - 1).gameObject;
                    objectState = selectedObject.GetComponent<ObjectState>();
                    objectInHand = objectState != null;

                    if (objectInHand)
                    {
                        StaticVariables.joystickMovementActive = false;
                        steamVRTeleport.SetActive(false);
                        snapTurn.SetActive(false);
                    }
                }

                if ((player.rightHand.transform.childCount == maxChildCountRightHand ||
                    player.leftHand.transform.childCount == maxChildCountLeftHand) && !usedAButton)
                    OpenHint(aButton, openSelectionMenuHint);

                //Two buttons can't be true at the same time, hence why trigger state is false since that's the first button that was activated and becomes false
                if (aButton.GetStateDown(hands) && !trigger.GetStateDown(hands))
                    if (player.rightHand.transform.childCount == maxChildCountRightHand ||
                        player.leftHand.transform.childCount == maxChildCountLeftHand)
                    {
                        usedAButton = true;
                        StaticVariables.activateMenuSelection = true;
                    }

                if (StaticVariables.activateMenuSelection && objectState != null)
                {
                    HideHint(aButton);

                    if (!usedJoystick)
                        OpenHint(joystickSelection, joystickSelectionHint);

                    JoystickSelection();
                }

                //Reset everything when you stop holding an object in hand
                if (!aButton.GetStateDown(hands) && !trigger.GetStateDown(hands))
                    if (player.rightHand.transform.childCount != maxChildCountRightHand &&
                        player.leftHand.transform.childCount != maxChildCountLeftHand)
                        ResetStatus();
            }
        }

        private void JoystickSelection()
        {
            //left/ask specialist
            if (joystickSelection.axis.x >= -1f && joystickSelection.axis.x < -0.2f && joystickSelection.axis.y >= -0.71f && joystickSelection.axis.y < 0.71f)
            {
                selection = "specialist";
                usedJoystick = true;
            }
            //top/interesting
            else if (joystickSelection.axis.x >= -0.71f && joystickSelection.axis.x <= 0.71f && joystickSelection.axis.y > 0.2f && joystickSelection.axis.y <= 1f)
            {
                selection = "interesting";
                usedJoystick = true;
            }
            //right/confiscate
            else if (joystickSelection.axis.x <= 1f && joystickSelection.axis.x > 0.2f && joystickSelection.axis.y >= -0.71f && joystickSelection.axis.y < 0.71f)
            {
                selection = "confiscate";
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
                        break;

                    case "confiscate":
                        objectState.MarkForConfiscate();
                        ResetStatus();
                        break;

                    case "specialist":
                        objectState.MarkForSpecialist();
                        ResetStatus();
                        break;

                    case "nothing":
                        objectState.MarkForNothing();
                        ResetStatus();
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
            objectInHand = false;
            timer = 0;
            seconds = 0;

            if (StaticVariables.activateMenuSelection)
                StaticVariables.activateMenuSelection = false;

            if (StaticVariables.locomotionStatus == "teleport")
            {
                steamVRTeleport.SetActive(true);
                snapTurn.SetActive(true);
            }
            else if (StaticVariables.locomotionStatus == "joystick")
                StaticVariables.joystickMovementActive = true;
        }
    }
}


