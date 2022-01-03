using Scripts.Weapon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public Firearms mainWeapon;
    public Firearms secondaryWeapon;

    private Firearms carriedWeapon;
    private FPCharacterControllerMovement fPCharacterControllerMovement;
    private IEnumerator waitingForHolsterEndCoroutine;

    private void Start()
    {
        carriedWeapon = mainWeapon;
        fPCharacterControllerMovement = FindObjectOfType<FPCharacterControllerMovement>();
        fPCharacterControllerMovement.SetupAnimator(carriedWeapon.gunAnimator);
    }

    private void Update()
    {
        if (carriedWeapon == null)
        {
            return;
        }

        SwapWeapon();
        if (Input.GetButton("FireMouse"))
        {
            carriedWeapon.HoldTrigger();
        }
        if (Input.GetButtonUp("FireMouse"))
        {
            carriedWeapon.ReleaseTrigger();
        }
        if (Input.GetButtonDown("Reload"))
        {
            carriedWeapon.ReloadAmmo();
        }
        if (Input.GetButtonDown("AimMouse"))
        {
            carriedWeapon.Aiming(true);
        }
        if (Input.GetButtonUp("AimMouse"))
        {
            carriedWeapon.Aiming(false);
        }
    }

    private void SwapWeapon()
    {
        if (Input.GetButtonDown("SwitchMainWeapon"))
        {
            if (carriedWeapon == mainWeapon)
            {
                return ;
            }
            if (carriedWeapon.gameObject.activeInHierarchy)
            {
                StartWaitingForHolsterEndCoroutine();
                carriedWeapon.gunAnimator.SetTrigger("holster");
            }
            else
            {
                SetupCarriedWeapon(mainWeapon);
            }
        }
        if (Input.GetButtonDown("SwitchSecondaryWeapon"))
        {
            if (carriedWeapon == secondaryWeapon)
            {
                return;
            }
            if (carriedWeapon.gameObject.activeInHierarchy)
            {
                StartWaitingForHolsterEndCoroutine();
                carriedWeapon.gunAnimator.SetTrigger("holster");
            }
            else
            {
                SetupCarriedWeapon(secondaryWeapon);
            }
        }
    }

    private void StartWaitingForHolsterEndCoroutine()
    {
        if (waitingForHolsterEndCoroutine == null)
        {
            waitingForHolsterEndCoroutine = WaitingForHolsterEnd();
        }
        StartCoroutine(waitingForHolsterEndCoroutine);
    }

    private IEnumerator WaitingForHolsterEnd()
    {
        while (true)
        {
            AnimatorStateInfo tmp_AnimatorStateInfo = carriedWeapon.gunAnimator.GetCurrentAnimatorStateInfo(0);
            if (tmp_AnimatorStateInfo.IsTag("holster"))
            {
                if (tmp_AnimatorStateInfo.normalizedTime >= 0.9f)
                {
                    var tmp_TargetWeapon = carriedWeapon == mainWeapon ? secondaryWeapon : mainWeapon;
                    SetupCarriedWeapon(tmp_TargetWeapon);
                    waitingForHolsterEndCoroutine = null;
                    yield break;
                }
            }
            yield return null;
        }
    }

    private void SetupCarriedWeapon(Firearms _targetWeapon)
    {
        carriedWeapon.gameObject.SetActive(false);
        carriedWeapon = _targetWeapon;
        carriedWeapon.gameObject.SetActive(true);
    }
}
