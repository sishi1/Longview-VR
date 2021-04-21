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

    private int childCount;
    private int maxChildCount;

    private void Start()
    {
        markObjectCanvas = markObjectUI.GetComponent<Canvas>();
        childCount = Player.instance.rightHand.gameObject.transform.childCount + 0;
        maxChildCount = 11;
    }

    private void Update()
    {
        //Checks whether the player has an object in hand
        if (Player.instance.rightHand.gameObject.transform.childCount != childCount)
        {
            Debug.Log("Child count: " + Player.instance.rightHand.gameObject.transform.childCount);
            //Attach the object in hand in the variable
            selectedObject = Player.instance.rightHand.transform.GetChild(Player.instance.rightHand.transform.childCount -1).gameObject;
            Debug.Log("Object selected: " + selectedObject);
            objectState = selectedObject.GetComponent<ObjectState>();
        }

        Debug.Log($"<b>markobject </b> A button state: {aButton.GetStateDown(rightHand)} Trigger state: {trigger.GetStateDown(rightHand)}");
        //Two buttons can't be true at the same time, hence why trigger state is false since that's the first button that was activated and becomes false
        if (aButton.GetStateDown(rightHand) && !trigger.GetStateDown(rightHand))
            if (Player.instance.rightHand.transform.childCount == maxChildCount)
                markObjectCanvas.enabled = true;
            else
                return;

        if (markObjectCanvas.enabled)
            JoystickSelection();
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
        markObjectCanvas.enabled = false;
    }
}
