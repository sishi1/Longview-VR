using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocomotionHover : MonoBehaviour
{
    private ChangeLocomotion changeLocomotion;
    private Transform layout;

    [Header("Type")]
    public bool walk;
    public bool teleport;

    private void Start()
    {
        changeLocomotion = FindObjectOfType<ChangeLocomotion>();
        layout = GetComponent<Transform>();
    }

    private void Update()
    {
        if ((walk && changeLocomotion.currentLocomotion.ToString() == "Walk") ||
            (teleport && changeLocomotion.currentLocomotion.ToString() == "Teleport"))
            layout.localScale = new Vector2(0.07f, 0.07f);
        else
            layout.localScale = new Vector2(0.05f, 0.05f);
    }
}
