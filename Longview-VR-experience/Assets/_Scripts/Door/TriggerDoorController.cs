using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Valve.VR.InteractionSystem
{
    public class TriggerDoorController : MonoBehaviour
    {
        private Player player = null;
        [Header("SteamVR Input")]
        public SteamVR_Input_Sources hands;
        public VR.SteamVR_Action_Boolean trigger;

        [SerializeField] private Animator doorAnim;

        private void Start()
        {
            player = Player.instance;
            doorAnim.SetBool("Open", false);
        }

        private void OnTriggerEnter(Collider other)
        {
            foreach (Hand hand in player.hands)
            {
                if (trigger.GetStateDown(hands))
                {
                    StaticVariables.doorUsed = true;

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
}

