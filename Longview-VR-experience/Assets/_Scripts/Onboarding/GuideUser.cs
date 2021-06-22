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
        MovementOptions,
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

    [Header("Guide objects")]
    [SerializeField] private GameObject introduction;
    [SerializeField] private GameObject switchLocomotion;
    [SerializeField] private GameObject joystick;
    [SerializeField] private GameObject teleport;
    [SerializeField] private GameObject interactionSystem;
    [SerializeField] private GameObject grabbingSystem;
    [SerializeField] private GameObject introGaze;
    [SerializeField] private GameObject gazeSystem;
    [SerializeField] private GameObject notebook;
    [SerializeField] private GameObject end;

    [Header("UI Components")]
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI text;



    private void Update()
    {
        if (Teleport.teleported)
            TutorialManager.isExplainingTeleport = false;

        if (!TutorialManager.endIntro)
            currentGuide = Guides.MovementOptions;

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
            case Guides.MovementOptions:
                introduction.SetActive(true);
                switchLocomotion.SetActive(false);
                joystick.SetActive(false);
                teleport.SetActive(false);
                interactionSystem.SetActive(false);
                grabbingSystem.SetActive(false);
                introGaze.SetActive(false);
                gazeSystem.SetActive(false);
                notebook.SetActive(false);
                end.SetActive(false);
                EnableLayout();
                break;

            case Guides.SwitchLocomotion:
                introduction.SetActive(false);
                switchLocomotion.SetActive(true);
                joystick.SetActive(false);
                teleport.SetActive(false);
                interactionSystem.SetActive(false);
                grabbingSystem.SetActive(false);
                introGaze.SetActive(false);
                gazeSystem.SetActive(false);
                notebook.SetActive(false);
                end.SetActive(false);
                EnableLayout();
                break;

            case Guides.Joystick:
                introduction.SetActive(false);
                switchLocomotion.SetActive(false);
                joystick.SetActive(true);
                teleport.SetActive(false);
                interactionSystem.SetActive(false);
                grabbingSystem.SetActive(false);
                introGaze.SetActive(false);
                gazeSystem.SetActive(false);
                notebook.SetActive(false);
                end.SetActive(false);
                EnableLayout();
                break;

            case Guides.Teleport:
                introduction.SetActive(false);
                switchLocomotion.SetActive(false);
                joystick.SetActive(false);
                teleport.SetActive(true);
                interactionSystem.SetActive(false);
                grabbingSystem.SetActive(false);
                introGaze.SetActive(false);
                gazeSystem.SetActive(false);
                notebook.SetActive(false);
                end.SetActive(false);
                EnableLayout();
                break;

            case Guides.InteractionSystem:
                introduction.SetActive(false);
                switchLocomotion.SetActive(false);
                joystick.SetActive(false);
                teleport.SetActive(false);
                interactionSystem.SetActive(true);
                grabbingSystem.SetActive(false);
                introGaze.SetActive(false);
                gazeSystem.SetActive(false);
                notebook.SetActive(false);
                end.SetActive(false);
                EnableLayout();
                break;

            case Guides.GrabbingSystem:
                introduction.SetActive(false);
                switchLocomotion.SetActive(false);
                joystick.SetActive(false);
                teleport.SetActive(false);
                interactionSystem.SetActive(false);
                grabbingSystem.SetActive(true);
                introGaze.SetActive(false);
                gazeSystem.SetActive(false);
                notebook.SetActive(false);
                end.SetActive(false);
                EnableLayout();
                break;

            case Guides.IntroGaze:
                introduction.SetActive(false);
                switchLocomotion.SetActive(false);
                joystick.SetActive(false);
                teleport.SetActive(false);
                interactionSystem.SetActive(false);
                grabbingSystem.SetActive(false);
                introGaze.SetActive(true);
                gazeSystem.SetActive(false);
                notebook.SetActive(false);
                end.SetActive(false);
                EnableLayout();
                break;

            case Guides.GazeSystem:
                introduction.SetActive(false);
                switchLocomotion.SetActive(false);
                joystick.SetActive(false);
                teleport.SetActive(false);
                interactionSystem.SetActive(false);
                grabbingSystem.SetActive(false);
                introGaze.SetActive(false);
                gazeSystem.SetActive(true);
                notebook.SetActive(false);
                end.SetActive(false);
                EnableLayout();
                break;

            case Guides.Notebook:
                introduction.SetActive(false);
                switchLocomotion.SetActive(false);
                joystick.SetActive(false);
                teleport.SetActive(false);
                interactionSystem.SetActive(false);
                grabbingSystem.SetActive(false);
                introGaze.SetActive(false);
                gazeSystem.SetActive(false);
                notebook.SetActive(true);
                end.SetActive(false);
                EnableLayout();
                break;

            case Guides.End:
                introduction.SetActive(false);
                switchLocomotion.SetActive(false);
                joystick.SetActive(false);
                teleport.SetActive(false);
                interactionSystem.SetActive(false);
                grabbingSystem.SetActive(false);
                introGaze.SetActive(false);
                gazeSystem.SetActive(false);
                notebook.SetActive(false);
                end.SetActive(true);
                EnableLayout();
                break;

            case Guides.None:
                introduction.SetActive(true);
                switchLocomotion.SetActive(true);
                joystick.SetActive(true);
                teleport.SetActive(true);
                interactionSystem.SetActive(true);
                interactionSystem.SetActive(true);
                grabbingSystem.SetActive(true);
                introGaze.SetActive(true);
                gazeSystem.SetActive(true);
                notebook.SetActive(true);
                end.SetActive(true);
                DisableLayout();
                break;

            default:
                Debug.LogErrorFormat("Something went wrong in the switch statement", currentGuide);
                break;
        }
    }

    private void EnableLayout()
    {
        image.enabled = true;
        text.enabled = true;
    }

    private void DisableLayout()
    {
        image.enabled = false;
        text.enabled = false;
    }
}
