using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSpring : MonoBehaviour
{
    public Vector2 minRecoilRange;
    public Vector2 maxRecoilRange;
    public float frequence;
    public float damp;

    private Transform cameraSpringTransform;
    private CameraSpringUtility cameraSpringUtility;

    private void Start()
    {
        cameraSpringUtility = new CameraSpringUtility(frequence, damp);
        cameraSpringTransform = transform;
    }

    private void Update()
    {
        cameraSpringUtility.UpdateSpring(Time.deltaTime, Vector3.zero);
        cameraSpringTransform.localRotation = Quaternion.Slerp(cameraSpringTransform.localRotation, 
                                              Quaternion.Euler(cameraSpringUtility.values), Time.deltaTime * 10);
    }

    public void StartCameraSpring()
    {
        cameraSpringUtility.values = 
            new Vector3(0, 
            UnityEngine.Random.Range(minRecoilRange.x, maxRecoilRange.x), 
            UnityEngine.Random.Range(minRecoilRange.y, maxRecoilRange.y));
    }
}
