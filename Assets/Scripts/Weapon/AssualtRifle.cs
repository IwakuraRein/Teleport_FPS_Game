using System.Collections;
using UnityEngine;

namespace Scripts.Weapon
{
    public class AssualtRifle : Firearms
    {
        private IEnumerator reloadAmmoCheckCoroutine;
        private FPMouseLook mouseLook;

        public GameObject bulletImpactPrefab;
        public bool isFiring { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            reloadAmmoCheckCoroutine = CheckReloadAmmoAnimationEnd();
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

        //private void Update()
        //{
        //    if (Input.GetButton("FireMouse") || Input.GetAxis("FireJoyStick") < 0)
        //    {
        //        DoAttack();
        //    }
        //    if (Input.GetButtonDown("Reload"))
        //    {
        //        Reload();
        //    }
        //    if (Input.GetButtonDown("AimMouse") || Input.GetAxis("AimJoyStick") > 0)
        //    {
        //        isAiming = true;
        //        Aim();
        //    }
        //    if (Input.GetButtonUp("AimMouse"))
        //    {
        //        isAiming = false;
        //        Aim();
        //    }
        //}

        protected void CreateBullet()
        {
            GameObject tmp_Bullet = Instantiate(bulletPrefab, muzzlePoint.position, muzzlePoint.rotation);
            tmp_Bullet.transform.eulerAngles += CalculateSpreadOffset();
            var tmp_BulletScript = tmp_Bullet.AddComponent<Bullet>();
            tmp_BulletScript.bulletSpeed = 100;
            if (tmp_Bullet != null)
            {
                Destroy(tmp_Bullet, 5);
            }
        }

        //protected override void Aim()
        //{
        //    gunAnimator.SetBool("Aim", isAiming);
        //    if (doAimCoroutine == null)
        //    {
        //        doAimCoroutine = DoAim();
        //        StartCoroutine(doAimCoroutine);
        //    }
        //    else
        //    {
        //        StopCoroutine(doAimCoroutine);
        //        doAimCoroutine = null;
        //        doAimCoroutine = DoAim();
        //        StartCoroutine(doAimCoroutine);
        //    }
        //}
    }
}
