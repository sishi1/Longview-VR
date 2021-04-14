using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Valve.VR
{
    public class TriggerDoorController : MonoBehaviour
    {
        [Header("SteamVR Input")]
        public SteamVR_Input_Sources rightHand;
        public SteamVR_Action_Boolean trigger;

        [SerializeField] private Animator doorAnim;

        private bool isDoorOpen;

        public void PlayAnimation()
        {
            if (!isDoorOpen)
            {
                doorAnim.Play("Door_open", 0, 0.0f);
                isDoorOpen = true;
            }
            else
            {
                doorAnim.Play("Door_closed", 0, 0.0f);
                isDoorOpen = false;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (trigger.GetStateDown(rightHand))
                PlayAnimation();
        }
    }
}

