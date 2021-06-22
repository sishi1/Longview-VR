using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Valve.VR.InteractionSystem
{
    public class SelectionHover : MonoBehaviour
    {
        private ChangeLocomotion changeLocomotion;
        private MarkObject markObject;
        private Transform layout;

        [Header("Selection type")]
        public bool interesting;
        public bool confiscate;
        public bool specialist;
        public bool nothing;

        [Header("Locomotion type")]
        public bool walk;
        public bool teleport;

        void Start()
        {
            changeLocomotion = FindObjectOfType<ChangeLocomotion>();
            markObject = FindObjectOfType<MarkObject>();
            layout = GetComponent<Transform>();
        }

        void Update()
        {
            if ((interesting && markObject.selection == "Interesting") ||
                (confiscate && markObject.selection == "Confiscate") ||
                (specialist && markObject.selection == "Specialist") ||
                (nothing && markObject.selection == "Nothing") ||
                (walk && changeLocomotion.currentLocomotion.ToString() == "Walk") ||
                (teleport && changeLocomotion.currentLocomotion.ToString() == "Teleport"))
            {
                layout.localScale = new Vector2(0.07f, 0.07f);
            }
            else
                layout.localScale = new Vector2(0.05f, 0.05f);
        }
    }
}