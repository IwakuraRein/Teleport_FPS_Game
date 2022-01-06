using System;
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
    private GameObject globalCamera;
    public ScoreManager scoreManager;

    public static event Action<float> Respawn;

    private bool hasDead=false;
    private void Start()
    {
        scoreManager = (ScoreManager)FindObjectOfType(typeof(ScoreManager));
        photonView = GetComponent<PhotonView>();
        if (photonView.IsMine)
        {
            globalCamera = GameObject.FindWithTag("GlobalCamera");
            if (globalCamera)
                globalCamera.SetActive(false);
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
            if (globalCamera)
                globalCamera.SetActive(true);

            Respawn?.Invoke(3);

            return;
        }

        Heath -= _damage;
    }


    private bool IsDeath()
    {
        if (Heath <= 0&&hasDead==false)
        {
            hasDead = true;
            scoreManager.AddOurPoint();
        }
        return Heath <= 0;
    }
}