using Scripts.Items;
using Scripts.Weapon;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public Firearms mainWeapon;
    public Firearms secondaryWeapon;
    public Transform worldCameraTransform;
    public float raycastMaxDistance;
    public LayerMask checkItemLayerMask;
    public List<Firearms> arms = new List<Firearms>();

    public Firearms carriedWeapon;
    private FPCharacterControllerMovement fPCharacterControllerMovement;
    private IEnumerator waitingForHolsterEndCoroutine;

    private void Start()
    {
        fPCharacterControllerMovement = FindObjectOfType<FPCharacterControllerMovement>();
        if (mainWeapon != null)
        {
            carriedWeapon = mainWeapon;
            fPCharacterControllerMovement.SetupAnimator(carriedWeapon.gunAnimator);
        }
    }

    private void Update()
    {
        CheckItem();

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

    private void CheckItem()
    {
        bool tmp_IsItem = Physics.Raycast(worldCameraTransform.position, worldCameraTransform.forward, out RaycastHit tmp_RaycastHit, raycastMaxDistance, checkItemLayerMask);

        if (tmp_IsItem)
        {
            if (Input.GetButtonDown("GetItem"))
            {
                bool tmp_HasItem = tmp_RaycastHit.collider.TryGetComponent(out BaseItem tmp_BaseItem);
                if (tmp_HasItem)
                {
                    if (tmp_BaseItem is FirearmsItem tmp_FirearmsItem)
                    {
                        foreach (Firearms tmp_arm in arms)
                        {
                            if (tmp_FirearmsItem.armsName.CompareTo(tmp_arm.name) == 0)
                            {
                                switch(tmp_FirearmsItem.currentFirearmsType)
                                {
                                    case FirearmsItem.FirearmsType.AssaultRifle:
                                        mainWeapon = tmp_arm;
                                        break;
                                    case FirearmsItem.FirearmsType.HandGun:
                                        secondaryWeapon = tmp_arm;
                                        break;
                                    default:
                                        throw new ArgumentOutOfRangeException();
                                }
                                SetupCarriedWeapon(tmp_arm);
                            }
                        }
                    }
                }
            }
        }
    }

    private void SwapWeapon()
    {
        if (Input.GetButtonDown("SwitchMainWeapon"))
        {
            if (mainWeapon == null)
            {
                return;
            }
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
            if (secondaryWeapon == null)
            {
                return;
            }
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
        if (carriedWeapon)
        {
            carriedWeapon.gameObject.SetActive(false);
        }
        carriedWeapon = _targetWeapon;
        carriedWeapon.gameObject.SetActive(true);
        fPCharacterControllerMovement.SetupAnimator(carriedWeapon.gunAnimator);
    }
}
