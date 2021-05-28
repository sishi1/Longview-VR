using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class ChangeLocomotion : MonoBehaviour
{
    [Header("SteamVR Input")]
    public SteamVR_Input_Sources leftHand;
    public SteamVR_Action_Boolean yButton;
    public SteamVR_Action_Vector2 changeLocomotion;

    [Header("Scripts")]
    [SerializeField] private GameObject steamVRTeleport;
    [SerializeField] private GameObject snapTurn;

    //[Header("Miscellaneous")]
    //public GameObject locomotionUI;
    //private Canvas locomotionCanvas;

    private bool activeTeleport = false;

    private void Awake()
    {
        StaticVariables.locomotionStatus = "teleport";
    }

    private void Start()
    {
        //locomotionCanvas = locomotionUI.GetComponent<Canvas>();
    }

    private void Update()
    {
        if (yButton.GetStateDown(leftHand))
            SelectLocomotion();
        //locomotionCanvas.enabled = true;

        //if (locomotionCanvas.enabled)
        //    SelectLocomotion();

        if (activeTeleport)
        {
            steamVRTeleport.SetActive(false);
            snapTurn.SetActive(false);
        } else if (!activeTeleport)
        {
            steamVRTeleport.SetActive(true);
            snapTurn.SetActive(true);
        }

    }

    private void SelectLocomotion()
    {
        if (!StaticVariables.joystickMovementActive)
        {
            activeTeleport = true;
            StaticVariables.joystickMovementActive = true;
            StaticVariables.locomotionStatus = "joystick";

        } else if (StaticVariables.joystickMovementActive)
        {
            activeTeleport = false;
            StaticVariables.joystickMovementActive = false;
            StaticVariables.locomotionStatus = "teleport";
        }

        //Debug.Log("x-as: " + changeLocomotion.axis.x);
        //Debug.Log("y-as: " + changeLocomotion.axis.y);
    }
}
