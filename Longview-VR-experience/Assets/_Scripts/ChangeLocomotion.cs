using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class ChangeLocomotion : MonoBehaviour
{
    [SerializeField] public enum Locomotion { Walk, Teleport, None };
    public Locomotion currentLocomotion;

    [Header("SteamVR Input")]
    public SteamVR_Input_Sources leftHand;
    public SteamVR_Action_Boolean yButton;
    public SteamVR_Action_Vector2 changeLocomotion;

    [Header("Scripts")]
    [SerializeField] private GameObject steamVRTeleport;
    [SerializeField] private GameObject snapTurn;

    [HideInInspector] public bool enableLayout = false;

    private void Start()
    {
        currentLocomotion = Locomotion.Teleport;
    }

    private void Update()
    {
        if (yButton.GetStateDown(leftHand))
        {
            currentLocomotion = Locomotion.None;
            enableLayout = true;
            TutorialManager.endIntro = true;
            TutorialManager.isExplainingSwitchLocomotion = true;

            if (TutorialManager.hasExplainedJoystick || TutorialManager.hasExplainedTeleport)
                TutorialManager.isExplainingSwitchLocomotion = false;
        }

        if (enableLayout)
        {
            DisableTeleport();
            SelectLocomotion();
        }

        if (currentLocomotion == Locomotion.Teleport)
            EnableTeleport();
        else
            DisableTeleport();
    }

    private void SelectLocomotion()
    {
        //Walking
        if (changeLocomotion.axis.x >= -0.82f && changeLocomotion.axis.x <= -0.55f && changeLocomotion.axis.y > 0.57f && changeLocomotion.axis.y <= 0.84f)
        {
            currentLocomotion = Locomotion.Walk;
        }
        //Teleporting
        else if (changeLocomotion.axis.x >= -0.14f && changeLocomotion.axis.x < 0.55f && changeLocomotion.axis.y >= 0.84f && changeLocomotion.axis.y < 0.99f)
        {
            currentLocomotion = Locomotion.Teleport;
        }

        SwitchLocomotion();
    }

    private void SwitchLocomotion()
    {
        if (changeLocomotion.axis.x == 0 && changeLocomotion.axis.y == 0)
        {
            switch (currentLocomotion)
            {
                case Locomotion.Walk:
                    DisableTeleport();
                    enableLayout = false;
                    if (TutorialManager.hasExplainedJoystick && !TutorialManager.isExplainingSwitchLocomotion)
                        break;

                    DisableGuide();
                    TutorialManager.isExplainingJoystick = true;
                    TutorialManager.isExplainingTeleport = false;

                    break;
                case Locomotion.Teleport:
                    EnableTeleport();
                    enableLayout = false;

                    if (TutorialManager.hasExplainedTeleport && !TutorialManager.isExplainingSwitchLocomotion)
                        break;

                    DisableGuide();
                    TutorialManager.isExplainingJoystick = false;
                    TutorialManager.isExplainingTeleport = true;

                    break;
            }
        }
    }

    private void EnableTeleport()
    {
        steamVRTeleport.SetActive(true);
        snapTurn.SetActive(true);
    }

    private void DisableTeleport()
    {
        steamVRTeleport.SetActive(false);
        snapTurn.SetActive(false);
    }

    private void DisableGuide()
    {
        TutorialManager.isExplainingSwitchLocomotion = false;
        TutorialManager.hasExplainedJoystick = true;
        TutorialManager.hasExplainedTeleport = true;
    }
}
