using System.Collections;
using UnityEngine;

namespace Scripts.Weapon
{
    public class AssualtRifle : Firearms
    {
        [SerializeField]
        private GameObject fineCross;
        [SerializeField]
        private GameObject portalIndicater;
        private IEnumerator reloadAmmoCheckCoroutine;
        private IEnumerator doAimCoroutine;
        private FPMouseLook mouseLook;
        
        public GameObject bulletImpactPrefab;
        public bool isFiring { get; private set; }

        protected override void Start()
        {
            base.Start();
            reloadAmmoCheckCoroutine = CheckReloadAmmoAnimationEnd();
            doAimCoroutine = DoAim();
            mouseLook = FindObjectOfType<FPMouseLook>();
        }

        protected override void Reload()
        {
            isReloading = true;
            if (currentMaxAmmoCarried > 0)
            {
                gunAnimator.SetLayerWeight(2, 1);
                gunAnimator.SetTrigger(currentAmmo > 0 ? "ReloadLeft" : "ReloadOutOf");

                firearmsReloadAudioSource.clip = currentAmmo > 0 ? firearmsAudioData.reloadLeftAudio : firearmsAudioData.reloadOutOfAudio;
                firearmsReloadAudioSource.Play();

                if (reloadAmmoCheckCoroutine == null)
                {
                    reloadAmmoCheckCoroutine = CheckReloadAmmoAnimationEnd();
                    StartCoroutine(reloadAmmoCheckCoroutine);
                }
                else
                {
                    StopCoroutine(reloadAmmoCheckCoroutine);
                    reloadAmmoCheckCoroutine = null;
                    reloadAmmoCheckCoroutine = CheckReloadAmmoAnimationEnd();
                    StartCoroutine(reloadAmmoCheckCoroutine);
                }
            }
        }

        protected override void Shooting()
        {
            if (isReloading)
            {
                return;
            }
            if (currentAmmo <= 0)
            {
                return;
            }
            if (!IsAllowShooting())
            {
                return;
            }
            muzzleParticle.Play();

            firearmsShootingAudioSource.clip = firearmsAudioData.shootingAudio;
            firearmsShootingAudioSource.Play();

            currentAmmo -= 1;
            gunAnimator.Play("Fire", isAiming ? 1 : 0, 0);
            CreateBullet();
            casingParticle.Play();
            mouseLook.FiringForTest();
            lastFireTime = Time.time;
        }

        private void Update()
        {
            if (Input.GetButton("FireMouse") || Input.GetAxis("FireJoyStick") < 0)
            {
                DoAttack();
            }
            if (Input.GetButtonDown("Reload"))
            {
                Reload();
            }
            if (Input.GetButtonDown("AimMouse") || Input.GetAxis("AimJoyStick") > 0)
            {
                fineCross.SetActive(false);
                portalIndicater.SetActive(false);
                isAiming = true;
                Aim();
            }
            if (Input.GetButtonUp("AimMouse"))
            {
                fineCross.SetActive(true);
                portalIndicater.SetActive(true);
                isAiming = false;
                Aim();
            }
        }

        protected void CreateBullet()
        {
            GameObject tmp_Bullet = Instantiate(bulletPrefab, muzzlePoint.position, muzzlePoint.rotation);
            tmp_Bullet.transform.eulerAngles += CalculateSpreadOffset();
            var tmp_BulletScript = tmp_Bullet.AddComponent<Bullet>();
            tmp_BulletScript.impactPrefab = bulletImpactPrefab;
            tmp_BulletScript.impactAudioData = impactAudioData;
            tmp_BulletScript.bulletSpeed = 100;
            if (tmp_Bullet != null)
            {
                Destroy(tmp_Bullet, 5);
            }
        }

        private IEnumerator CheckReloadAmmoAnimationEnd()
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

        private IEnumerator DoAim()
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

        protected override void Aim()
        {
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
    }
}