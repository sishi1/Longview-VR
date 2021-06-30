using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Valve.VR.InteractionSystem;

public class GuideUser : MonoBehaviour
{
    [SerializeField]
    private enum Guides
    {
        Introduction,
        SwitchLocomotion,
        Teleport,
        Joystick,
        InteractionSystem,
        GrabbingSystem,
        IntroGaze,
        GazeSystem,
        Notebook,
        End,
        None
    };

    [SerializeField] private Guides currentGuide;

    [SerializeField] private GameObject[] tutorialCanvas;

    private void Update()
    {
        if (Teleport.teleported)
            TutorialManager.isExplainingTeleport = false;

        if (!TutorialManager.endIntro)
            currentGuide = Guides.Introduction;

        else if (TutorialManager.isExplainingSwitchLocomotion)
            currentGuide = Guides.SwitchLocomotion;

        else if (TutorialManager.isExplainingJoystick)
            currentGuide = Guides.Joystick;

        else if (TutorialManager.isExplainingTeleport)
            currentGuide = Guides.Teleport;

        else if (TutorialManager.isExplainingInteractionSystem)
            currentGuide = Guides.InteractionSystem;

        else if (TutorialManager.isExplainingGrabbingSystem)
            currentGuide = Guides.GrabbingSystem;

        else if (TutorialManager.isExplainingIntroGaze)
            currentGuide = Guides.IntroGaze;

        else if (TutorialManager.isExplainingGazeSystem)
            currentGuide = Guides.GazeSystem;

        else if (TutorialManager.isExplainingNotebook)
            currentGuide = Guides.Notebook;

        else if (TutorialManager.endTutorial)
            currentGuide = Guides.End;

        else
            currentGuide = Guides.None;


        switch (currentGuide)
        {
            case Guides.Introduction:
                DisableTutorialCanvas();
                tutorialCanvas[0].SetActive(true);
                break;

            case Guides.SwitchLocomotion:
                DisableTutorialCanvas();
                tutorialCanvas[1].SetActive(true);
                break;

            case Guides.Joystick:
                DisableTutorialCanvas();
                tutorialCanvas[3].SetActive(true);
                break;

            case Guides.Teleport:
                DisableTutorialCanvas();
                tutorialCanvas[2].SetActive(true);
                break;

            case Guides.InteractionSystem:
                DisableTutorialCanvas();
                tutorialCanvas[4].SetActive(true);
                break;

            case Guides.GrabbingSystem:
                DisableTutorialCanvas();
                tutorialCanvas[5].SetActive(true);
                break;

            case Guides.IntroGaze:
                DisableTutorialCanvas();
                tutorialCanvas[6].SetActive(true);
                break;

            case Guides.GazeSystem:
                DisableTutorialCanvas();
                tutorialCanvas[7].SetActive(true);
                break;

            case Guides.Notebook:
                DisableTutorialCanvas();
                tutorialCanvas[8].SetActive(true);
                break;

            case Guides.End:
                DisableTutorialCanvas();
                tutorialCanvas[9].SetActive(true);
                TutorialManager.startTimer = true;
                break;

            case Guides.None:
                DisableTutorialCanvas();
                break;

            default:
                Debug.LogErrorFormat("Something went wrong in the switch statement", currentGuide);
                break;
        }
    }

    private void DisableTutorialCanvas()
    {
        int children = transform.childCount;
        for (int i = 0; i < children; i++)
            transform.GetChild(i).gameObject.SetActive(false);
    }
}
