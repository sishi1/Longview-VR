using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderingMenu : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;


    // Start is called before the first frame update
    void Start()
    {
        this.spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (StaticVariables.activateMenuSelection)
            this.spriteRenderer.enabled = true;
        else
            this.spriteRenderer.enabled = false;
    }
}
