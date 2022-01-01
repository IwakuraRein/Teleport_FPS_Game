using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimingCtrl: MonoBehaviour
{
    public Transform Arms;
    public Transform AimTarget;
    public float AimTargetDistance = 5f;
    private Vector3 localPosition;
    private Quaternion localRotation;

    private void Update()
    {
        localRotation = Arms.localRotation;
        localPosition = localRotation * Vector3.forward * AimTargetDistance;
        AimTarget.localPosition = localPosition;
    }

}
