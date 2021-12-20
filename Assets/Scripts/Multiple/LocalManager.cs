using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class LocalManager : MonoBehaviour
{
    public List<MonoBehaviour> LocalScripts;
    public Camera Gun_camera;
    public Camera Main_camera;
    public Camera Portal_camera;
    private PhotonView photonView;

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
        if (photonView.IsMine) return;

        Gun_camera.enabled = false;
        Main_camera.enabled = false;
        Portal_camera.enabled = false;

        foreach (MonoBehaviour behaviour in LocalScripts)
        {
            behaviour.enabled = false;
        }
    }
}
