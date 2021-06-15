using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class PlayerController : MonoBehaviour
{
    [Header("SteamVR Input")]
    public SteamVR_Action_Vector2 touchpadInput;
    public SteamVR_Action_Vector2 turnInput;

    [Header("Miscellaneous")]
    [SerializeField] float speed;

    private ChangeLocomotion changeLocomotion;

    private void Start()
    {
        changeLocomotion = FindObjectOfType<ChangeLocomotion>();
    }

    private void Update()
    {
        if (changeLocomotion.currentLocomotion.ToString() == "Walk" && StaticVariables.joystickMovementActive)
        {
            Move();

            //Threshold to start turning
            if (turnInput.axis.x > 0.2f || turnInput.axis.x < -0.2f)
            {
                Turn();
            }
        }

    }

    private void Move()
    {
        //Gets the controller input 
        Vector3 movementDirection = Player.instance.hmdTransform.TransformDirection(new Vector3(0, 0, touchpadInput.axis.y));
        //Execute the movement based on input, speed and time
        transform.position += Vector3.ProjectOnPlane(Time.deltaTime * movementDirection * speed, Vector3.up);
    }

    private void Turn()
    {
        //Where we're going to rotate to 
        var rotationAmount = transform.eulerAngles.y + turnInput.axis.x;
        //Gives new direction of where we're looking
        var directionVector = new Vector3(transform.eulerAngles.x, rotationAmount, transform.eulerAngles.z);
        //Execute the new direction
        transform.rotation = Quaternion.Euler(directionVector);
    }
}
