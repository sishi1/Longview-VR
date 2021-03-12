using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class PlayerController : MonoBehaviour
{
    public SteamVR_Action_Vector2 touchpadInput;

    // Start is called before the first frame update
    void Start()
    {

    }

    private void FixedUpdate()
    {
        Vector3 movementDirection = Player.instance.hmdTransform.TransformDirection(new Vector3(touchpadInput.axis.x, 0, touchpadInput.axis.y));
        transform.position += Vector3.ProjectOnPlane(Time.deltaTime * movementDirection * 2.0f, Vector3.up);
    }
}