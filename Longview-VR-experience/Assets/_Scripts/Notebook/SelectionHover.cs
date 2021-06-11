using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Valve.VR.InteractionSystem
{
    public class SelectionHover : MonoBehaviour
    {
        public GameObject objectHightlight;
        private MarkObject markObject;
        private Transform t;
        private bool hovered;

        public bool interesting;
        public bool confiscate;
        public bool specialist;
        public bool nothing;

        void Start()
        {
            markObject = objectHightlight.GetComponent<MarkObject>();
            t = GetComponent<Transform>();
        }

        void Update()
        {
            if ((interesting && markObject.selection == "interesting") ||
                (confiscate && markObject.selection == "confiscate") ||
                (specialist && markObject.selection == "specialist") ||
                (nothing && markObject.selection == "nothing"))
            {
                hovered = true;
                t.localScale = new Vector2(0.07f, 0.07f);
            }
            else if (hovered)
            {
                hovered = false;
            }

            if (!hovered)
            {
                t.localScale = new Vector2(0.05f, 0.05f);
            }
        }
    }
}