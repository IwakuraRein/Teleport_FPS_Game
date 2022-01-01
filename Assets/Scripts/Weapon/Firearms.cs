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

        internal Animator gunAnimator;

        protected AnimatorStateInfo gunStateInfo;
        protected int currentAmmo;
        protected int currentMaxAmmoCarried;
        protected float lastFireTime;
        protected float originFOV;
        protected bool isReloading = false;
        protected bool isAiming = false;
        protected bool isHoldingTrigger;

        private IEnumerator doAimCoroutine;

        protected virtual void Awake()
        {
            currentAmmo = ammoInMag;
            currentMaxAmmoCarried = maxAmmoCarried;
            gunAnimator = GetComponent<Animator>();
            originFOV = eyeCamera.fieldOfView;
            doAimCoroutine = DoAim();
        }

        public void DoAttack()
        {
            Shooting();
        }

        protected abstract void Shooting();
        protected abstract void Reload();
        //protected abstract void Aim();

        protected bool IsAllowShooting()
        {
            return (Time.time - lastFireTime > 1 / fireRate);
        }

        protected Vector3 CalculateSpreadOffset()
        {
            float tmp_SpreadPercent = spreadAngle / eyeCamera.fieldOfView;
            return tmp_SpreadPercent * UnityEngine.Random.insideUnitCircle;
        }

        internal void Aiming(bool _isAiming)
        {
            isAiming = _isAiming;
            gunAnimator.SetBool("Aim", isAiming);
            if (doAimCoroutine == null)
            {
                doAimCoroutine = DoAim();
                StartCoroutine(doAimCoroutine);
            }
            else
            {
                StopCoroutine(doAimCoroutine);
                doAimCoroutine = null;
                doAimCoroutine = DoAim();
                StartCoroutine(doAimCoroutine);
            }
        }

        protected IEnumerator CheckReloadAmmoAnimationEnd()
        {
            isReloading = true;
            while (true)
            {
                yield return null;
                gunStateInfo = gunAnimator.GetCurrentAnimatorStateInfo(2);
                if (gunStateInfo.IsTag("ReloadAmmo"))
                {
                    if (gunStateInfo.normalizedTime >= 0.9f)
                    {
                        int tmp_NeedAmmoCount = ammoInMag - currentAmmo;
                        int tmp_RemainingAmmo = currentMaxAmmoCarried - tmp_NeedAmmoCount;
                        if (tmp_RemainingAmmo <= 0)
                        {
                            currentAmmo += currentMaxAmmoCarried;
                        }
                        else
                        {
                            currentAmmo = ammoInMag;
                        }

                        currentMaxAmmoCarried = tmp_RemainingAmmo <= 0 ? 0 : tmp_RemainingAmmo;

                        isReloading = false;
                        yield break;
                    }
                }
            }
        }

        protected IEnumerator DoAim()
        {
            while (true)
            {
                yield return null;

                float tmp_CurrentFOV = 0;
                eyeCamera.fieldOfView =
                    Mathf.SmoothDamp(
                        eyeCamera.fieldOfView,
                        isAiming ? aimingFOV : originFOV,
                        ref tmp_CurrentFOV,
                        Time.deltaTime * 2);

            }
        }

        internal void HoldTrigger()
        {
            DoAttack();
            isHoldingTrigger = true;
        }

        internal void ReleaseTrigger()
        {
            isHoldingTrigger = false;
        }

        internal void ReloadAmmo()
        {
            Reload();
        }
    }
}