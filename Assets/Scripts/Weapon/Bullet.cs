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

        private Vector3 prevPosition;
        private RaycastHit thisHit;
        private Vector3 tmpPos1, tmpPos2,tmptmphit, newPrevPos;
        private bool firstLaunch=true;

        private void Start()
        {
            prevPosition=transform.position;
        }

        private void Update()
        {
            prevPosition = transform.position;
            transform.Translate(0, 0, bulletSpeed * Time.deltaTime);
            // Vector3 dir;
            // if (firstLaunch) {dir = new Vector3(0, 0, 1); firstLaunch = false;}
            // else dir = (transform.position - prevPosition).normalized;
            // transform.Translate(dir * bulletSpeed * Time.deltaTime);
            var dir = (transform.position - prevPosition).normalized;

            if (!Shoot(dir)) return;

            var tmp_BulletEffect =
                Instantiate(impactPrefab, thisHit.point, Quaternion.LookRotation(thisHit.normal, Vector3.up));

            Destroy(tmp_BulletEffect, 3);

            var tmp_TagsWithAudio = impactAudioData.impactTagsWithAudios.Find(
                (_tmp_AudioData) => { return _tmp_AudioData.tag.Equals(thisHit.collider.tag); }
                );
            if (tmp_TagsWithAudio != null)
            {
                int tmp_Length = tmp_TagsWithAudio.impactAudioClips.Count;
                AudioClip tmp_AudioClip = tmp_TagsWithAudio.impactAudioClips[Random.Range(0, tmp_Length)];
                AudioSource.PlayClipAtPoint(tmp_AudioClip, thisHit.point);
            }

            Destroy(gameObject);
        }
        private bool Shoot(Vector3 dir)
        {
            // LayerMask mask = 1 << 你需要开启的Layers层。
            // LayerMask mask = 0 << 你需要关闭的Layers层。
            if (!Physics.Raycast(prevPosition,
                dir,
                out RaycastHit tmp_Hit,
                (transform.position - prevPosition).magnitude)) return false;
            else if (tmp_Hit.collider.tag == "Portal")
            {
                var inPortal = tmp_Hit.collider.GetComponent<Portal>();
                if (inPortal == null) return false;
                var outPortal = inPortal.OtherPortal;
                tmptmphit=tmp_Hit.point;
                Debug.Log("before enter into portal: " + prevPosition.ToString() + " --> " + transform.position.ToString());

                // Update position of raycast origin with small offset.
                tmpPos1 = transform.position;
                Vector3 relativePos = inPortal.transform.InverseTransformPoint(transform.position);
                relativePos = Quaternion.Euler(0.0f, 180.0f, 0.0f) * relativePos;
                transform.position = outPortal.transform.TransformPoint(relativePos);

                tmpPos2 = prevPosition;
                relativePos = new Vector3();
                relativePos = inPortal.transform.InverseTransformPoint(tmp_Hit.point);
                relativePos = Quaternion.Euler(0.0f, 180.0f, 0.0f) * relativePos;
                prevPosition = outPortal.transform.TransformPoint(relativePos);
                dir = (transform.position - prevPosition).normalized;
                if ((dir*0.5f).magnitude > (transform.position-prevPosition).magnitude)
                    transform.Translate(0, 0, bulletSpeed * Time.deltaTime);
                prevPosition = prevPosition+dir*0.5f;

                // Update rotation.
                Quaternion relativeRot = Quaternion.Inverse(inPortal.transform.rotation) * transform.rotation;
                relativeRot = Quaternion.Euler(0.0f, 180.0f, 0.0f) * relativeRot;
                transform.rotation = outPortal.transform.rotation * relativeRot;
                
                newPrevPos = prevPosition;
                //Time.timeScale=0.005f;

                //Debug.Log("after enter into portal: " + tmpPos2.ToString() + " --> " + prevPosition.ToString());

                // Update direction of raycast.
                // Vector3 relativeDir = inPortal.transform.InverseTransformDirection(dir);
                // relativeDir = Quaternion.Euler(0.0f, 180.0f, 0.0f) * relativeDir;
                // dir = outPortal.transform.TransformDirection(relativeDir);

                return Shoot(dir);
            }
            else
            {
                thisHit = tmp_Hit;
                return true;
            }
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(tmptmphit, 0.1f); //portal hit point

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(tmpPos2, 0.1f); //old prev
            Gizmos.color = Color.black;
            Gizmos.DrawSphere(newPrevPos, 0.1f); //new

            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(tmpPos1, 0.1f); //old bullet
            Gizmos.color = Color.gray;
            Gizmos.DrawSphere(transform.position, 0.1f); //new

            // Gizmos.color = Color.yellow;
            // Gizmos.DrawLine(tmpPos2, tmpPos1);
            // Gizmos.color = Color.green;
            // Gizmos.DrawLine(newPrevPos, transform.position);
        }
    }
}