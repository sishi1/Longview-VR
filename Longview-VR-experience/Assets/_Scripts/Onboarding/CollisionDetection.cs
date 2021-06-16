using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Guide interaction system"))
            if (!TutorialManager.hasExplainedInteractionSystem)
                TutorialManager.isExplainingInteractionSystem = true;
    }
}
