using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class MarkObject : MonoBehaviour
{
    [Header("SteamVR Input")]
    public SteamVR_Input_Sources rightHand;
    public SteamVR_Action_Boolean aButton;
    public SteamVR_Action_Vector2 joystickSelection;

    [HideInInspector]
    public Interactable interactable;
    [HideInInspector]
    public ObjectState objectState;
    [HideInInspector]
    public string selection;

    private GameObject selectedObject;
    public GameObject markObjectUI;
    private Canvas markObjectCanvas;

    private bool choiceLocked;

    private void Start()
    {
        markObjectCanvas = markObjectUI.GetComponent<Canvas>();
        interactable = FindObjectOfType<Interactable>();
    }

    private void Update()
    {
        //if a object is in hand
        if (interactable.holdingObject)
        {
            //if the object is different than previous object
            if (this.gameObject != selectedObject)
            {
                selectedObject = this.gameObject;
                objectState = selectedObject.GetComponent<ObjectState>();
                
            }

            if (aButton.GetStateDown(rightHand))
                markObjectCanvas.enabled = true;

            JoystickSelection();

        }
        else
            markObjectCanvas.enabled = false;
    }

    private void JoystickSelection()
    {
        //left/interesting
        if (joystickSelection.axis.x >= -1f && joystickSelection.axis.x < -0.2f && joystickSelection.axis.y >= -0.71f && joystickSelection.axis.y < 0.71f)
        {
            choiceLocked = true;
            selection = "interesting";
        }
        //top/confiscate
        else if (joystickSelection.axis.x >= -0.71f && joystickSelection.axis.x <= 0.71f && joystickSelection.axis.y > 0.2f && joystickSelection.axis.y <= 1f)
        {
            choiceLocked = true;
            selection = "confiscate";
        }
        //right/ask specialist
        else if (joystickSelection.axis.x <= 1f && joystickSelection.axis.x > 0.2f && joystickSelection.axis.y >= -0.71f && joystickSelection.axis.y < 0.71f)
        {
            choiceLocked = true;
            selection = "specialist";
        }
        //bottom/nothing
        else if (joystickSelection.axis.x <= 0.71f && joystickSelection.axis.x > -0.71f && joystickSelection.axis.y >= -1f && joystickSelection.axis.y < -0.2f)
        {
            choiceLocked = true;
            selection = "nothing";
        }

        if (choiceLocked)
        {
            if (joystickSelection.axis.x == 0 && joystickSelection.axis.y == 0)
            {
                switch(selection)
                {
                    case "interesting":
                        objectState.MarkForInterest();
                        Debug.Log(selectedObject + "Send to notebook as interest");
                        ResetStatus();
                        markObjectCanvas.enabled = false;
                        break;

                    case "confiscate":
                        //objectState.MarkForTaking();
                        Debug.Log(selectedObject + "Send to notebook as confiscate");
                        ResetStatus();
                        markObjectCanvas.enabled = false;
                        break;

                    case "specialist":
                        //objectState.MarkForSpecialist();
                        Debug.Log(selectedObject + "Send to notebook as specialist");
                        ResetStatus();
                        markObjectCanvas.enabled = false;
                        break;

                    case "nothing":
                        //objectState.MarkForNothing();
                        Debug.Log(selectedObject + "Send to notebook as nothing");
                        ResetStatus();
                        markObjectCanvas.enabled = false;
                        break;

                    case null:
                        Debug.LogErrorFormat("Something went wrong in the switch statement", selection);
                        break;
                }
            }
        }
    }

    private void ResetStatus()
    {
        selection = "";
        choiceLocked = false;
    }
}
