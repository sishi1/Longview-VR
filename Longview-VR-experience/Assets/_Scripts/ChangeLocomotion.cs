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

    private bool activeTeleport = false;
    private bool activateSwitch = false;

    private void Awake()
    {
        StaticVariables.locomotionStatus = "teleport";
    }

    private void Update()
    {
        Debug.Log("Activate switch locomotion: " + StaticVariables.activateSwitchLocomotion);

        if (yButton.GetStateDown(leftHand))
        {
            StaticVariables.activateSwitchLocomotion = true;
            activateSwitch = true;
        }

        if (activateSwitch)
            SelectLocomotion();

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

    private void SwitchLocomotion()
    {
        if (changeLocomotion.axis.x == 0 && changeLocomotion.axis.y == 0)
        {
            switch (StaticVariables.locomotionSwitchStatus)
            {
                case "walk":
                    activeTeleport = true;
                    StaticVariables.joystickMovementActive = true;
                    StaticVariables.locomotionStatus = "joystick";
                    ResetStatus();
                    break;

                case "teleport":
                    activeTeleport = false;
                    StaticVariables.joystickMovementActive = false;
                    ResetStatus();
                    break;

                default:
                    break;
            }
        }
    }

    private void SelectLocomotion()
    {

        //Walking
        if (changeLocomotion.axis.x >= -0.82f && changeLocomotion.axis.x <= -0.55f && changeLocomotion.axis.y > 0.57f && changeLocomotion.axis.y <= 0.84f)
        {
            StaticVariables.locomotionSwitchStatus = "walk";
        }
        //Teleporting
        else if (changeLocomotion.axis.x >= -0.14f && changeLocomotion.axis.x < 0.55f && changeLocomotion.axis.y >= 0.84f && changeLocomotion.axis.y < 0.99f)
        {
            StaticVariables.locomotionSwitchStatus = "teleport";
        }

        SwitchLocomotion();
    }

    private void ResetStatus()
    {
        activateSwitch = false;
        StaticVariables.activateSwitchLocomotion = false;
    }
}
