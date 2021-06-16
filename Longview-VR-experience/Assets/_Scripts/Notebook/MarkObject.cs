using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Valve.VR.InteractionSystem
{
    public class MarkObject : MonoBehaviour
    {
        private ChangeLocomotion changeLocomotion;
        private Player player = null;

        [Header("Miscellaneous")]
        public Transform controller;

        [SerializeField] private LineRenderer line;
        [SerializeField] private LayerMask checkLayer;

        private GameObject selectedObject;

        [Header("Game objects")]
        [SerializeField] private GameObject steamVRTeleport;
        [SerializeField] private GameObject snapTurn;

        [Header("SteamVR Input")]
        public SteamVR_Input_Sources hands;
        public VR.SteamVR_Action_Boolean aButton;
        public SteamVR_Action_Vector2 joystickSelection;
        public VR.SteamVR_Action_Boolean trigger;
        public VR.SteamVR_Action_Boolean grip;

        [Header("Hints")]
        [SerializeField] private string openSelectionMenuHint;
        [SerializeField] private string joystickSelectionHint;

        [HideInInspector] public ObjectState objectState;
        [HideInInspector] public string selection;
        [HideInInspector] public bool enableLayout;

        private int childCountRightHand;
        private int childCountLeftHand;
        private int maxChildCountRightHand;
        private int maxChildCountLeftHand;

        private bool usedAButton;
        private bool usedJoystick;
        private bool objectInHand = false;
        private bool objectHit = false;
        private bool activateGaze = false;
        [HideInInspector] public bool enableFeedback = false;

        private float timer = 0f;
        private int seconds = 0;
        private readonly int activationTime = 1;

        private void Start()
        {
            changeLocomotion = FindObjectOfType<ChangeLocomotion>();
            player = Player.instance;

            childCountRightHand = player.rightHand.gameObject.transform.childCount + 0;
            childCountLeftHand = player.leftHand.gameObject.transform.childCount + 0;

            maxChildCountRightHand = 10;
            maxChildCountLeftHand = 8;
        }

        private void Update()
        {
            if (grip.GetStateDown(hands))
                if (activateGaze)
                    activateGaze = false;
                else
                    activateGaze = true;

            if (activateGaze)
            {
                if (!TutorialManager.hasExplainedIntroGaze)
                {
                    TutorialManager.isExplainingIntroGaze = false;
                    TutorialManager.hasExplainedIntroGaze = true;
                    TutorialManager.isExplainingGazeSystem = true;
                }

                GazeSystem();
            }
            else
                GrabbingSystem();
        }

        private void GazeSystem()
        {
            line.enabled = true;

            if (Physics.Raycast(controller.position, controller.forward, out RaycastHit hit, StaticVariables.lineLength, checkLayer))
            {
                enableFeedback = true;
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
                        enableLayout = true;
                        enableFeedback = false;
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

        private void GrabbingSystem()
        {
            line.enabled = false;

            //if (selectedObject != null)
            //    Debug.Log($"<b>markobject </b> Object state: {(objectState == null ? "null" : objectState.ToString())} Selected object: {selectedObject.name}");


            //Checks whether the player has an object in hand
            if (player.rightHand.gameObject.transform.childCount != childCountRightHand && !objectInHand)
            {
                //Attach the object in hand in the variable
                selectedObject = player.rightHand.transform.GetChild(player.rightHand.transform.childCount - 1).gameObject;
                objectState = selectedObject.GetComponent<ObjectState>();
                objectInHand = objectState != null;
            }

            if (player.leftHand.gameObject.transform.childCount != childCountLeftHand && !objectInHand)
            {
                //Attach the object in hand in the variable
                selectedObject = player.leftHand.transform.GetChild(player.leftHand.transform.childCount - 1).gameObject;
                objectState = selectedObject.GetComponent<ObjectState>();
                objectInHand = objectState != null;
            }

            if (objectInHand)
            {
                if (!TutorialManager.hasExplainedGrabbingSystem)
                {
                    TutorialManager.isExplainingInteractionSystem = false;
                    TutorialManager.hasExplainedInteractionSystem = true;
                    TutorialManager.isExplainingGrabbingSystem = true;
                }

                StaticVariables.joystickMovementActive = false;
                steamVRTeleport.SetActive(false);
                snapTurn.SetActive(false);
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
                    enableLayout = true;
                }

            if (enableLayout && objectState != null)
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

        private void JoystickSelection()
        {
            //left/ask specialist
            if (joystickSelection.axis.x >= -1f && joystickSelection.axis.x < -0.2f && joystickSelection.axis.y >= -0.71f && joystickSelection.axis.y < 0.71f)
            {
                selection = "Specialist";
                usedJoystick = true;
            }
            //top/interesting
            else if (joystickSelection.axis.x >= -0.71f && joystickSelection.axis.x <= 0.71f && joystickSelection.axis.y > 0.2f && joystickSelection.axis.y <= 1f)
            {
                selection = "Interesting";
                usedJoystick = true;
            }
            //right/confiscate
            else if (joystickSelection.axis.x <= 1f && joystickSelection.axis.x > 0.2f && joystickSelection.axis.y >= -0.71f && joystickSelection.axis.y < 0.71f)
            {
                selection = "Confiscate";
                usedJoystick = true;
            }
            //bottom/nothing
            else if (joystickSelection.axis.x <= 0.71f && joystickSelection.axis.x > -0.71f && joystickSelection.axis.y >= -1f && joystickSelection.axis.y < -0.2f)
            {
                selection = "Nothing";
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
                    case "Interesting":
                        objectState.MarkForInterest();
                        DisableGuides();
                        ResetStatus();
                        break;

                    case "Confiscate":
                        objectState.MarkForConfiscate();
                        DisableGuides();
                        ResetStatus();
                        break;

                    case "Specialist":
                        objectState.MarkForSpecialist();
                        DisableGuides();
                        ResetStatus();
                        break;

                    case "Nothing":
                        objectState.MarkForNothing();
                        DisableGuides();
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

        private void DisableGuides()
        {
            if (activateGaze)
            {
                if (!TutorialManager.hasExplainedGazeSystem)
                {
                    TutorialManager.isExplainingGazeSystem = false;
                    TutorialManager.hasExplainedGazeSystem = true;
                    TutorialManager.isExplainingNotebook = true;
                }
            }
            else
            {
                if (!TutorialManager.hasExplainedGrabbingSystem)
                {
                    TutorialManager.isExplainingGrabbingSystem = false;
                    TutorialManager.hasExplainedGrabbingSystem = true;
                    TutorialManager.isExplainingIntroGaze = true;
                }
            }

        }

        private void ResetStatus()
        {
            selection = "";
            objectInHand = false;
            objectHit = false;

            timer = 0;
            seconds = 0;

            enableFeedback = false;
            enableLayout = false;

            if (changeLocomotion.currentLocomotion.ToString() == "Teleport")
            {
                steamVRTeleport.SetActive(true);
                snapTurn.SetActive(true);
            }
            else if (changeLocomotion.currentLocomotion.ToString() == "Walk")
                StaticVariables.joystickMovementActive = true;
        }
    }
}


