using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StaticVariables
{
    //Hints variables
    public static bool noteBookUsed = false;
    public static bool doorUsed = false;

    //Onboarding audio that isn't necessary anymore
    public static bool isOnboardingPlaying = false;

    //Locomotion variables
    public static bool joystickMovementActive = false;
    public static string locomotionStatus;

    //Switch system variables
    public static bool activateMenuSelection = false;
    public static bool activateSwitchLocomotion = false;
    public static bool activateSwitchInteraction = false;

    public static string locomotionSwitchStatus = "";

    //Linerenderer variable
    public static float lineLength = 0f;
}
