using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Weapon
{
    public abstract class Firearms : MonoBehaviour, IWeapon
    {
        public Transform muzzlePoint;
        public Transform casingPoint;
        public ParticleSystem muzzleParticle;
        public ParticleSystem casingParticle;
        public int ammoInMag;
        public int maxAmmoCarried;
        public float fireRate;
        public GameObject bulletPrefab;
        public AudioSource firearmsShootingAudioSource;
        public AudioSource firearmsReloadAudioSource;
        public FirearmsAudioData firearmsAudioData;
        public Camera eyeCamera;
        public int aimingFOV;
        public ImpactAudioData impactAudioData;
        public float spreadAngle;

        protected Animator gunAnimator;
        protected AnimatorStateInfo gunStateInfo;
        protected int currentAmmo;
        protected int currentMaxAmmoCarried;
        protected float lastFireTime;
        protected float originFOV;
        protected bool isReloading = false;
        protected bool isAiming = false;

        protected virtual void Start()
        {
            currentAmmo = ammoInMag;
            currentMaxAmmoCarried = maxAmmoCarried;
            gunAnimator = GetComponent<Animator>();
            originFOV = eyeCamera.fieldOfView;
        }

        public void DoAttack()
        {
            Shooting();
        }

        protected abstract void Shooting();
        protected abstract void Reload();
        protected abstract void Aim();

        protected bool IsAllowShooting()
        {
            return (Time.time - lastFireTime > 1 / fireRate);
        }

        protected Vector3 CalculateSpreadOffset()
        {
            float tmp_SpreadPercent = spreadAngle / eyeCamera.fieldOfView;
            return tmp_SpreadPercent * UnityEngine.Random.insideUnitCircle;
        }
    }
}