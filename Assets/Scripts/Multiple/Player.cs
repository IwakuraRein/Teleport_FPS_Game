﻿using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityTemplateProjects.MultiplayerScripts;
using Random = UnityEngine.Random;

//Rhoton组件 Remote Procedure Call
[RequireComponent(typeof(PhotonView))]
public class Player : MonoBehaviour, IDamager
{
    public int Heath;
    public string PlayerPrefabName;
    private PhotonView photonView;
    private GameObject GlobalCamera;

    public static event Action<float> Respawn;

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
        if (photonView.IsMine)
        {
            GlobalCamera = GameObject.FindWithTag("GlobalCamera");
            if (GlobalCamera)
                GlobalCamera.SetActive(false);
        }
    }

    public void TakeDamage(int _damage)
    {
        photonView.RpcSecure("RPC_TakeDamage", RpcTarget.All, true, _damage);
    }


    [PunRPC]
    private void RPC_TakeDamage(int _damage, PhotonMessageInfo _info)
    {
        if (IsDeath() && photonView.IsMine)
        {
            //gameObject.SetActive(false);
            PhotonNetwork.Destroy(this.gameObject);
            Destroy(this.gameObject);
            if (GlobalCamera)
                GlobalCamera.SetActive(true);

            Respawn?.Invoke(30);

            return;
        }

        Heath -= _damage;
    }


    private bool IsDeath()
    {
        return Heath <= 0;
    }
}