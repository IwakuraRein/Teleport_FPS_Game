using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Weapon
{
    public class Bullet : MonoBehaviour
    {
        public float bulletSpeed;
        public GameObject impactPrefab;
        public ImpactAudioData impactAudioData;

        private Transform bulletTransform;
        private Vector3 prevPosition;

        private void Start()
        {
            prevPosition = transform.position;
            bulletTransform = transform;
        }

        private void Update()
        {
            prevPosition = bulletTransform.position;
            bulletTransform.Translate(0, 0, bulletSpeed * Time.deltaTime);

            if (!Physics.Raycast(prevPosition,
                (bulletTransform.position - prevPosition).normalized,
                out RaycastHit tmp_Hit,
                (bulletTransform.position - prevPosition).magnitude)) return;
            var tmp_BulletEffect =
                Instantiate(impactPrefab, tmp_Hit.point, Quaternion.LookRotation(tmp_Hit.normal, Vector3.up));
            Destroy(tmp_BulletEffect, 3);

            var tmp_TagsWithAudio = impactAudioData.impactTagsWithAudios.Find(
                (_tmp_AudioData) => { return _tmp_AudioData.tag.Equals(tmp_Hit.collider.tag); }
                );
            if (tmp_TagsWithAudio == null) return;
            int tmp_Length = tmp_TagsWithAudio.impactAudioClips.Count;
            AudioClip tmp_AudioClip = tmp_TagsWithAudio.impactAudioClips[Random.Range(0, tmp_Length)];
            AudioSource.PlayClipAtPoint(tmp_AudioClip, tmp_Hit.point);
        }
    }

}