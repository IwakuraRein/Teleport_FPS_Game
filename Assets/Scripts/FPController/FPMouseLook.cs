using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPMouseLook : MonoBehaviour
{
    [SerializeField] private Transform characterTransform;
    private Vector3 cameraRotation = new Vector3(0, 0, 0);
    public bool syncThisFrame = false;
    public Vector2 MaxMinAngle;
    public float MouseSensitivity;
    public AnimationCurve recoilCurve;
    public Vector2 recoilRange;
    public float recoilFadeOutTime;

    private Vector2 currentRecoil;
    private float currentRecoilTime;
    private Transform cameraTransform;
    private CameraSpring cameraSpring;

    // Start is called before the first frame update
    void Start()
    {
        cameraTransform = transform;
        cameraSpring = GetComponentInChildren<CameraSpring>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!syncThisFrame) {
            var tmp_MouseX = Input.GetAxis("Mouse X");
            var tmp_MouseY = Input.GetAxis("Mouse Y");

            cameraRotation.x -= tmp_MouseY * MouseSensitivity;
            cameraRotation.y += tmp_MouseX * MouseSensitivity;

            CalculateRecoilOffset();

            cameraRotation.x -= currentRecoil.x;
            cameraRotation.y += currentRecoil.y;

            cameraRotation.x = Mathf.Clamp(cameraRotation.x, MaxMinAngle.x, MaxMinAngle.y);

            cameraTransform.rotation = Quaternion.Euler(cameraRotation.x, cameraRotation.y, 0);
            characterTransform.rotation = Quaternion.Euler(0, cameraRotation.y, 0);
            //Debug.Log(cameraRotation.ToString() + cameraTransform.rotation.eulerAngles.ToString());
        }
        else {
            if (cameraTransform.rotation.eulerAngles.x > 294)
                cameraRotation.x = cameraTransform.rotation.eulerAngles.x-360;
            else cameraRotation.x = cameraTransform.rotation.eulerAngles.x;
            cameraRotation.y = cameraTransform.rotation.eulerAngles.y;
            //cameraRotation.z = cameraTransform.rotation.eulerAngles.y;
            syncThisFrame = false;
        }
    }

    private void CalculateRecoilOffset()
    {
        currentRecoilTime += Time.deltaTime;
        float tmp_RecoilFraction = currentRecoilTime / recoilFadeOutTime;
        float tmp_RecoilValue = recoilCurve.Evaluate(tmp_RecoilFraction);
        currentRecoil = Vector2.Lerp(Vector2.zero, currentRecoil, tmp_RecoilValue);
    }

    public void FiringForTest()
    {
        currentRecoil += recoilRange;
        cameraSpring.StartCameraSpring();
        currentRecoilTime = 0;
    }
}
