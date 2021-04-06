using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class MarkObject : MonoBehaviour
{
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

    [Header("Raycast")]
    public LayerMask checkLayer;
    [SerializeField]
    public float range;

    [HideInInspector]
    public ObjectState objectState;
    [HideInInspector]
    public string selection;

    private bool triggerPressed;


    private void Start()
    {
        markObjectCanvas = markObjectUI.GetComponent<Canvas>();
    }

    private void Update()
    {
        RaycastHit hit;

        //Checks whether the raycast found a object of the specified layer 
        if (Physics.Raycast(controller.position, controller.forward, out hit, range, checkLayer))
        {
            if (trigger.GetStateDown(rightHand))
                triggerPressed = true;

            if (hit.collider.gameObject != selectedObject)
            {
                selectedObject = hit.collider.gameObject;
                objectState = selectedObject.GetComponent<ObjectState>();
            }

            if (aButton.GetStateDown(rightHand) && triggerPressed)
                markObjectCanvas.enabled = true;

            JoystickSelection();

        }
        else
        {
            markObjectCanvas.enabled = false;
            triggerPressed = false;
        }
    }

    private void JoystickSelection()
    {
        //left/interesting
        if (joystickSelection.axis.x >= -1f && joystickSelection.axis.x < -0.2f && joystickSelection.axis.y >= -0.71f && joystickSelection.axis.y < 0.71f)
        {
            selection = "interesting";
        }
        //top/confiscate
        else if (joystickSelection.axis.x >= -0.71f && joystickSelection.axis.x <= 0.71f && joystickSelection.axis.y > 0.2f && joystickSelection.axis.y <= 1f)
        {
            selection = "confiscate";
        }
        //right/ask specialist
        else if (joystickSelection.axis.x <= 1f && joystickSelection.axis.x > 0.2f && joystickSelection.axis.y >= -0.71f && joystickSelection.axis.y < 0.71f)
        {
            selection = "specialist";
        }
        //bottom/nothing
        else if (joystickSelection.axis.x <= 0.71f && joystickSelection.axis.x > -0.71f && joystickSelection.axis.y >= -1f && joystickSelection.axis.y < -0.2f)
        {
            selection = "nothing";
        }

        ConfirmSelection();
    }

    private void ConfirmSelection()
    {
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

    private void ResetStatus()
    {
        selection = "";
    }
}
