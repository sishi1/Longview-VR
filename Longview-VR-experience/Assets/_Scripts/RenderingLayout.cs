using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Valve.VR.InteractionSystem
{
    public class RenderingLayout : MonoBehaviour
    {
        private enum Layout { Locomotion, MarkingObjects, None};
        private Layout currentLayout;

        [Header("Gameobject UI")]
        [SerializeField] private GameObject switchLocomotion;
        [SerializeField] private GameObject markGameObject;

        private SpriteRenderer spriteRenderer;

        private ChangeLocomotion changeLocomotion;
        private MarkObject markObject;

        // Start is called before the first frame update
        void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            changeLocomotion = FindObjectOfType<ChangeLocomotion>();
            markObject = FindObjectOfType<MarkObject>();
            currentLayout = Layout.None;
        }

        // Update is called once per frame
        void Update()
        {
            if (changeLocomotion.enableLayout)
                currentLayout = Layout.Locomotion;
            else if (markObject.enableLayout)
                currentLayout = Layout.MarkingObjects;
            else
                currentLayout = Layout.None;

            switch (currentLayout)
            {
                case Layout.Locomotion:
                    markGameObject.SetActive(false);
                    switchLocomotion.SetActive(true);
                    spriteRenderer.enabled = true;
                    break;

                case Layout.MarkingObjects:
                    markGameObject.SetActive(true);
                    switchLocomotion.SetActive(false);
                    spriteRenderer.enabled = true;
                    break;

                case Layout.None:
                    markGameObject.SetActive(true);
                    switchLocomotion.SetActive(true);
                    spriteRenderer.enabled = false;
                    break;

                default:
                    Debug.LogErrorFormat("Something went wrong in the switch statement", currentLayout);
                    break;
            }
        }
    }

}
