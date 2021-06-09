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

        //Timer en seconds beginnen allebei met 0, want je wilt pas aftellen wanneer we onze raycast wijzen naar een object
        private float timer = 0f;
        private int seconds = 0;

        //Deze variabel is anders. Hier heb ik het getal 5 neergezet. activationTime heeft dus een getal waarde van 5
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

                //Gaat kijken of er een collision plaatsvindt met een object
                if (Physics.Raycast(controller.position, controller.forward, out RaycastHit hit, StaticVariables.lineLength, checkLayer))
                {
                    Debug.Log(timer);

                    //Telt langzamerhand in milliseconden omhoog
                    timer += Time.deltaTime;

                    //Als de timer variabel 1 seconden heeft bereikt dan gaat het door naar de code in de { }
                    if (timer >= 1f)
                    {
                        //Deze variabel staat iets bovenaan, maar het gaat telkens met 1 omhoog als de if statement hierboven op true staat
                        seconds++;

                        //De timer wordt teruggezet op 0 zodat het vanaf het begin weer begint met tellen
                        timer = 0;
                    }

                    //Zodra de seconden die we hebben geteld gelijk staat aan de variabel activationTime, dan gaan we door met de code in de { }
                    if (seconds >= activationTime)
                    {
                        //selectedObject wordt gelijkgesteld aan het gameobject wat onze raycast heeft gevonden
                        selectedObject = hit.collider.gameObject;

                        //objectState weet nu welk gameobject (bijvoorbeeld phone) hij naar de notebook kan sturen
                        objectState = selectedObject.GetComponent<ObjectState>();

                        //Checkt of objectState wel een object heeft of niet
                        objectHit = objectState != null;

                        //Als objectState een object heeft en dus niet null is, dan gaan we verder in de { }
                        if (objectHit)
                        {
                            //Hier zetten we de selections aan (de layout die Kaj heeft gemaakt)
                            StaticVariables.activateSelection = true;

                            //Hiermee zorgen we ervoor dat we de layout kunnen selecteren a.d.h.v. onze joystick positie
                            JoystickSelection();

                            //Dit allemaal zorgt ervoor dat we niet kunnen teleporteren, bewegen en draaien
                            StaticVariables.joystickMovementActive = false;
                            steamVRTeleport.SetActive(false);
                            snapTurn.SetActive(false);
                        }
                    }
                }
                else
                    //Als de raycast niet meer met een object collide, dan resetten we alles om ervoor te zorgen dat we "fresh" van start gaan
                    ResetStatus();
            }
            else
            {
                line.enabled = false;

                if (selectedObject != null)
                    Debug.Log($"<b>markobject </b> Object state: {(objectState == null ? "null" : objectState.ToString())} Selected object: {selectedObject.name}");

                Debug.Log($"<b>markobject </b> Right standard child count: {childCountRightHand} Right child count: {player.rightHand.gameObject.transform.childCount}");
                Debug.Log($"<b>markobject </b> Left standard child count: {childCountLeftHand} Left child count: {player.leftHand.gameObject.transform.childCount}");

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
                        StaticVariables.activateSelection = true;
                    }

                if (StaticVariables.activateSelection && objectState != null)
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

            if (StaticVariables.activateSelection)
                StaticVariables.activateSelection = false;

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


