using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Valve.VR.InteractionSystem
{
    public class Audio : MonoBehaviour
    {
        [Header("Audio")]
        public AudioSource audioSource;

        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
            StartCoroutine(PlayAudio());
        }

        private IEnumerator PlayAudio()
        {
            yield return new WaitForSeconds(6);

            while (StaticVariables.isOnboardingPlaying)
                yield return null;

            audioSource.Play();
        }
    }
}
