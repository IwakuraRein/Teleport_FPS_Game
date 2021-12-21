using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Photon.Pun;

public class LocalManager : MonoBehaviour
{
    public List<MonoBehaviour> LocalScripts;
    public List<Renderer> TPRenderers;
    public Camera Gun_camera;
    public Camera Main_camera;
    public Camera Portal_camera;
    private PhotonView photonView;
    public GameObject FPArms;

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
        if (photonView.IsMine)
        {
            gameObject.AddComponent<AudioListener>();
            return;
        }

        FPArms.SetActive(false);
        Gun_camera.enabled = false;
        Main_camera.enabled = false;
        Portal_camera.enabled = false;

        foreach (MonoBehaviour behaviour in LocalScripts)
        {
            behaviour.enabled = false;
        }
        foreach (Renderer tpRenderer in TPRenderers)
        {
            tpRenderer.shadowCastingMode = ShadowCastingMode.On;
        }
    }
}
