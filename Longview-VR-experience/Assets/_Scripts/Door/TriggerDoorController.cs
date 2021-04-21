using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Valve.VR.InteractionSystem
{
    public class TriggerDoorController : MonoBehaviour
    {
        [Header("SteamVR Input")]
        public SteamVR_Input_Sources rightHand;
        public SteamVR_Action_Boolean trigger;

        [SerializeField] private Animator doorAnim;

        public static bool used = false;

        private void Awake()
        {
            doorAnim.SetBool("Open", false);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (trigger.GetStateDown(rightHand))
            {
                used = true;

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

