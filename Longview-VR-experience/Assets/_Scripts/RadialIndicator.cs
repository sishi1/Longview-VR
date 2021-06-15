using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Valve.VR.InteractionSystem
{
    public class RadialIndicator : MonoBehaviour
    {
        private MarkObject markObject;

        [Header("UI")]
        [SerializeField] private Image radialIndicatorUI = null;
        public GameObject layout;
        private Canvas canvas;

        private float indicatorTimer;
        private float defaultTime = 0f;

        private void Start()
        {
            markObject = FindObjectOfType<MarkObject>();
            canvas = layout.GetComponent<Canvas>();
            indicatorTimer = defaultTime;
        }

        private void Update()
        {
            if (markObject.enableFeedback)
            {
                canvas.enabled = true;
                radialIndicatorUI.enabled = true;
                radialIndicatorUI.fillAmount = indicatorTimer;
                indicatorTimer += Time.deltaTime;

                if (indicatorTimer >= 1f)
                    ResetStatus();
            }
            else
                ResetStatus();
        }

        private void ResetStatus()
        {
            radialIndicatorUI.fillAmount = 0f;
            indicatorTimer = defaultTime;
            radialIndicatorUI.enabled = false;
            canvas.enabled = false;
        }
    }
}
