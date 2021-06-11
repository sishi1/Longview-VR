using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocomotionHover : MonoBehaviour
{
    private Transform layout;

    public bool walk;
    public bool teleport;

    private void Start()
    {
        layout = GetComponent<Transform>();
    }

    private void Update()
    {
        if ((walk && StaticVariables.locomotionSwitchStatus == "walk") ||
            (teleport && StaticVariables.locomotionSwitchStatus == "teleport"))
            layout.localScale = new Vector2(0.07f, 0.07f);
        else
            layout.localScale = new Vector2(0.05f, 0.05f);
    }
}
