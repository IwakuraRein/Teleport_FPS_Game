﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using RenderPipeline = UnityEngine.Rendering.RenderPipelineManager;

public class MainCam : MonoBehaviour
{
    [SerializeField]
    private Portal[] portals = new Portal[2];

    [SerializeField]
    private GameObject portalCameraObject;

    [SerializeField]
    private int iterations = 7;

    private RenderTexture tempTexture1;
    private RenderTexture tempTexture2;

    private Camera mainCamera;
    private Camera portalCamera;

    private Quaternion halfTurn = Quaternion.Euler(0.0f, 180.0f, 0.0f);
    public bool insidePortal = false;
    public Portal inPortal;

    private void Awake()
    {
        mainCamera = GetComponent<Camera>();
        portalCamera = portalCameraObject.GetComponent<Camera>();
        tempTexture1 = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.ARGB32);
        tempTexture2 = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.ARGB32);
    }

    private void Start()
    {
        portals[0].Renderer.material.mainTexture = tempTexture1;
        portals[1].Renderer.material.mainTexture = tempTexture2;
    }

    private void OnEnable()
    {
        RenderPipeline.beginCameraRendering += UpdateCamera;
    }

    private void OnDisable()
    {
        RenderPipeline.beginCameraRendering -= UpdateCamera;
    }

    void UpdateCamera(ScriptableRenderContext SRC, Camera camera)
    {
        if (!portals[0].IsPlaced || !portals[1].IsPlaced)
        {
            return;
        }

        if (portals[0].Renderer.isVisible)
        {
            if (!insidePortal) portalCamera.targetTexture = tempTexture1;
            else portalCamera.targetTexture = null;
            for (int i = iterations - 1; i >= 0; --i)
            {
                RenderCamera(portals[0], portals[1], i, SRC);
            }
        }

        if(portals[1].Renderer.isVisible)
        {
            if (!insidePortal) portalCamera.targetTexture = tempTexture2;
            else portalCamera.targetTexture = null;
            for (int i = iterations - 1; i >= 0; --i)
            {
                RenderCamera(portals[1], portals[0], i, SRC);
            }
        }
    }

    private void RenderCamera(Portal inPortal, Portal outPortal, int iterationID, ScriptableRenderContext SRC)
    {
        // 放置虚拟摄像机的原理：先把当前摄像机的世界坐标转换成传送门的坐标系里去，然后绕Y轴旋转180度
        Transform inTransform = inPortal.transform;
        Transform outTransform = outPortal.transform;

        Transform cameraTransform = portalCamera.transform;
        cameraTransform.position = transform.position;
        cameraTransform.rotation = transform.rotation;

        for(int i = 0; i <= iterationID; ++i)
        {
            // Position the camera behind the other portal.
            Vector3 relativePos = inTransform.InverseTransformPoint(cameraTransform.position); //到世界坐标
            relativePos = Quaternion.Euler(0.0f, 180.0f, 0.0f) * relativePos;
            cameraTransform.position = outTransform.TransformPoint(relativePos);

            // Rotate the camera to look through the other portal.
            Quaternion relativeRot = Quaternion.Inverse(inTransform.rotation) * cameraTransform.rotation;
            relativeRot = Quaternion.Euler(0.0f, 180.0f, 0.0f) * relativeRot;
            cameraTransform.rotation = outTransform.rotation * relativeRot;
        }

        // Set the camera's oblique view frustum.
        // 摄像机的视锥是倾斜的！！！
        Plane p = new Plane(-outTransform.forward, outTransform.position);
        Vector4 clipPlaneWorldSpace = new Vector4(p.normal.x, p.normal.y, p.normal.z, p.distance);
        Vector4 clipPlaneCameraSpace =
            Matrix4x4.Transpose(Matrix4x4.Inverse(portalCamera.worldToCameraMatrix)) * clipPlaneWorldSpace;

        var newMatrix = mainCamera.CalculateObliqueMatrix(clipPlaneCameraSpace);
        portalCamera.projectionMatrix = newMatrix;

        // Render the camera to its render target.
        UniversalRenderPipeline.RenderSingleCamera(SRC, portalCamera);
    }

    // 玩家摄像机进入传送门后需要调整。
    private void OnTriggerEnter(Collider other)
    {
        inPortal = other.GetComponent<Portal>();
        if (inPortal != null)
        {
            insidePortal = true;
            portalCamera.enabled = true;
            mainCamera.enabled = false;
            Debug.Log("camera enter portal");
        }
    }
    // private void OnTriggerExit(Collider other)
    // {
    //     var obj = other.GetComponent<Portal>();
    //     if (obj != null)
    //     {
    //         insidePortal = false;
    //         inPortal = null;
    //         mainCamera.enabled = true;
    //         portalCamera.enabled = false;
    //         Debug.Log("camera exit portal");
    //     }
    // }
    public void ExitPortal(){
        insidePortal = false;
        inPortal = null;
        mainCamera.enabled = true;
        portalCamera.enabled = false;
        Debug.Log("camera exit portal");
    }

}
