using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] private Image image;

    private float value;
    private readonly float defaultValue = 0f;
    private readonly float endTime = 1f;
    private readonly float waitTime = 1f / 8f;

    private void Start()
    {
        value = defaultValue;
    }

    private void Update()
    {
        if (TutorialManager.startTimer)
        {
            image.enabled = true;
            image.fillAmount = value;
            value += waitTime * Time.deltaTime;

            if (value >= endTime)
            {
                TutorialManager.endTutorial = false;
                image.enabled = false;
            }
        }
    }
}
