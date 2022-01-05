using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Random = UnityEngine.Random;

public class Launcher : MonoBehaviourPunCallbacks
{
    public InputField RoomName;

    public string PlayerPrefabName;
    private bool connectedToMaster;
    private bool joinedRoom;

    public void ConnectToMaster()
    {
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.GameVersion = "Alpha";
    }

    public void CreateRoom()
    {
        if (!connectedToMaster || joinedRoom) return;
        PhotonNetwork.CreateRoom(RoomName.text, new Photon.Realtime.RoomOptions() { MaxPlayers = 16 }, default);
    }

    public void JoinRoom()
    {
        if (!connectedToMaster || joinedRoom) return;
        PhotonNetwork.JoinRoom(RoomName.text);
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        connectedToMaster = true;
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        joinedRoom = true;
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log(message: "Joined Room");
        StartSpawn(0);
        Player.Respawn += StartSpawn;   
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        Player.Respawn -= StartSpawn;
    }

    private void StartSpawn(float _timeToSpawn)
    {
        StartCoroutine(routine: WaitToInstantiatatePlayer(0));
    }

    private IEnumerator WaitToInstantiatatePlayer(float _timeSpawn)
    {
        yield return new WaitForSeconds(_timeSpawn);
        PhotonNetwork.Instantiate(PlayerPrefabName, new Vector3(Random.Range(-25, 25), 3, Random.Range(-25, 25)), Quaternion.identity);
    }
}
