using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Valve.VR.InteractionSystem
{
    public class OnboardingAudio : MonoBehaviour
    {
        private Player player = null;

        [Header("Audio")]
        public AudioSource audioSource;


        private void Start()
        {
            player = Player.instance;
            audioSource = GetComponent<AudioSource>();
            
            StartCoroutine(PlayAudio());
        }

        private void Update()
        {
            this.transform.position = player.trackingOriginTransform.position;

            if (audioSource.isPlaying)
                StaticVariables.isOnboardingPlaying = true;
            else
                StaticVariables.isOnboardingPlaying = false;
        }

        private IEnumerator PlayAudio()
        {
            yield return new WaitForSeconds(3);

            audioSource.Play();
        }
    }

}