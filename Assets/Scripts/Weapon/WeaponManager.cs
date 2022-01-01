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
            carriedWeapon.gameObject.SetActive(false);
            carriedWeapon = mainWeapon;
            carriedWeapon.gameObject.SetActive(true);
            fPCharacterControllerMovement.SetupAnimator(carriedWeapon.gunAnimator);
        }
        if (Input.GetButtonDown("SwitchSecondaryWeapon"))
        {
            carriedWeapon.gameObject.SetActive(false);
            carriedWeapon = secondaryWeapon;
            carriedWeapon.gameObject.SetActive(true);
            fPCharacterControllerMovement.SetupAnimator(carriedWeapon.gunAnimator);
        }
    }
}
