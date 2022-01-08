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
    public GameObject PreviewPortal;
    public GameObject HUD;
    public GameObject EnemyBody;
    public GameObject PlayingUI;

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
        if (photonView.IsMine)
        {
            gameObject.AddComponent<AudioListener>();
            for (int i = 0; i < EnemyBody.transform.childCount; i++)
            {
                var child = EnemyBody.transform.GetChild(i).gameObject;
                child.layer = LayerMask.NameToLayer("PlayerBody");
            }
            return;
        }

        HUD.SetActive(false);
        PreviewPortal.SetActive(false);
        FPArms.SetActive(false);
        PlayingUI.SetActive(false);
        Gun_camera.enabled = false;
        Main_camera.enabled = false;
        Portal_camera.enabled = false;
        GetComponent<PortalPlacement>().enabled = false;
        EnemyBody.layer = LayerMask.NameToLayer("PlayerBody2");
        for (int i = 0; i < EnemyBody.transform.childCount; i++)
        {
            var child = EnemyBody.transform.GetChild(i).gameObject;
            child.layer = LayerMask.NameToLayer("PlayerBody2");
        }

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
