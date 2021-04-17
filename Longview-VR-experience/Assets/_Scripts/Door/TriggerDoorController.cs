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

        private void Awake()
        {
            doorAnim.SetBool("Open", false);
        }

        private void OnTriggerEnter(Collider other)
        {

            if (trigger.GetStateDown(rightHand))
            {
                if (!doorAnim.GetBool("Open"))
                {
                    doorAnim.SetBool("Open", true);
                }
                else
                    doorAnim.SetBool("Open", false);
            }
        }
    }
}

