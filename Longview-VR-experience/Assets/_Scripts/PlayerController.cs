using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class PlayerController : MonoBehaviour
{
    //Public means that you can access the variable (touchpadInput and turnInput) from other scripts and it shows in the inspector
    public SteamVR_Action_Vector2 touchpadInput;
    public SteamVR_Action_Vector2 turnInput;

    //[SerializeField] means that you can edit the value of the variable (speed) in the inspector and it'll update in the script
    [SerializeField] float speed;


    private void FixedUpdate()
    {
        Move();

        //Threshold to start turning
        if (turnInput.axis.x > 0.2f || turnInput.axis.x < -0.2f)
        {
            Turn();
        }

    }

    private void Move()
    {
        //Gets the controller input for left and right
        Vector3 movementDirection = Player.instance.hmdTransform.TransformDirection(new Vector3(touchpadInput.axis.x, 0, touchpadInput.axis.y));
        //Execute the movement based on input, speed and time
        transform.position += Time.deltaTime * movementDirection * speed;
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