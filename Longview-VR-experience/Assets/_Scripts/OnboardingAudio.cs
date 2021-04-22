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
        }

        private IEnumerator PlayAudio()
        {
            yield return new WaitForSeconds(5);

            audioSource.Play();
        }
    }

}