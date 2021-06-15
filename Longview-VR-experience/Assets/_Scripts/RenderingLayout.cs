using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Valve.VR.InteractionSystem
{
    public class RenderingLayout : MonoBehaviour
    {
        [Header("Gameobject UI")]
        [SerializeField] private GameObject switchLocomotion;
        [SerializeField] private GameObject markGameObject;

        [Header("UI")]
        public GameObject layout;
        private SpriteRenderer spriteRenderer;

        public GameObject player;
        private ChangeLocomotion changeLocomotion;
        private MarkObject markObject;

        private string whichLayout = "";

        // Start is called before the first frame update
        void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            changeLocomotion = player.GetComponentInChildren<ChangeLocomotion>();
            markObject = FindObjectOfType<MarkObject>();
        }

        // Update is called once per frame
        void Update()
        {
            Debug.Log($"<b>RenderingLayout</b> Change locomotion: {changeLocomotion.enableLayout} MarkObject: {markObject.enableLayout}");

            if (changeLocomotion.enableLayout)
                whichLayout = "Locomotion";
            else if (markObject.enableLayout)
                whichLayout = "MarkObjects";
            else
                whichLayout = "";

            switch (whichLayout)
            {
                case "Locomotion":
                    markGameObject.SetActive(false);
                    switchLocomotion.SetActive(true);
                    spriteRenderer.enabled = true;
                    break;

                case "MarkObjects":
                    markGameObject.SetActive(true);
                    switchLocomotion.SetActive(false);
                    spriteRenderer.enabled = true;
                    break;

                default:
                    markGameObject.SetActive(true);
                    switchLocomotion.SetActive(true);
                    spriteRenderer.enabled = false;
                    break;
            }
        }
    }

}
