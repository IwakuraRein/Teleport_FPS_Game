using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootstepListener : MonoBehaviour
{
    public FootstepAudioData footstepAudioData;
    public AudioSource footstepAudioSource;
    public LayerMask layerMask;

    private CharacterController characterController;
    private Transform footstepTransform;
    private float nextPlayTime;


    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        footstepTransform = transform;
    }

    private void FixedUpdate()
    {
        if (characterController.isGrounded)
        {
            if (characterController.velocity.normalized.magnitude > 0.1f)
            {
                nextPlayTime += Time.fixedDeltaTime;

                bool tmp_isHit = Physics.Linecast(footstepTransform.position, 
                    footstepTransform.position + Vector3.down * (characterController.height / 2 + characterController.skinWidth - characterController.center.y), 
                    out RaycastHit tmp_HitInfo, layerMask);

                if (tmp_isHit)
                {
                    foreach (var tmp_AudioElement in footstepAudioData.footstepAudios)
                    {
                        if (tmp_HitInfo.collider.CompareTag(tmp_AudioElement.tag))
                        {
                            if (nextPlayTime >= tmp_AudioElement.Delay)
                            {

                                int tmp_AudioCount = tmp_AudioElement.audioClips.Count;
                                int tmp_AudioIndex = UnityEngine.Random.Range(0, tmp_AudioCount);
                                AudioClip tmp_FootstepAudioClip = tmp_AudioElement.audioClips[tmp_AudioIndex];
                                footstepAudioSource.clip = tmp_FootstepAudioClip;
                                footstepAudioSource.Play();
                                nextPlayTime = 0;
                                break;
                            }
                        }
                    }
                }
            }
        }
    }
}
