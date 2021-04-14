using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Valve.VR
{
    public class RaycastDoor : MonoBehaviour
    {
        [Header("SteamVR Input")]
        public SteamVR_Input_Sources rightHand;
        public SteamVR_Action_Boolean trigger;

        [SerializeField] private float length;
        [SerializeField] private LayerMask layermask;

        private TriggerDoorController raycastedObj;

        private bool doOnce;

        private const string doorTag = "Door";

        private void Update()
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position, transform.forward, out hit, length, layermask))
            {
                if (hit.collider.CompareTag(doorTag))
                {
                    if (!doOnce)
                    {
                        raycastedObj = hit.collider.gameObject.GetComponent<TriggerDoorController>();
                    }

                    doOnce = true;

                    if (trigger.GetStateDown(rightHand))
                    {
                        Debug.Log("Holding the doorknob");
                        raycastedObj.PlayAnimation();
                    }
                }
            } 
            else
            {
                doOnce = false;
            }
        }
    }
}
