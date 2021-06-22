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
        [SerializeField] private GameObject layout;
        private Canvas canvas;

        private float indicatorTimer;
        private readonly float defaultTime = 0f;
        private readonly float resetFill = 0f;
        private readonly float endTime = 1f;

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

                if (indicatorTimer >= endTime)
                    ResetStatus();
            }
            else
                ResetStatus();
        }

        private void ResetStatus()
        {
            radialIndicatorUI.fillAmount = resetFill;
            indicatorTimer = defaultTime;
            radialIndicatorUI.enabled = false;
            canvas.enabled = false;
        }
    }
}
