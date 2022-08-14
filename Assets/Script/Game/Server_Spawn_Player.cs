using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Photon.Pun;
public class Server_Spawn_Player : MonoBehaviourPun
{
    public GameObject playerPrefab;

    void Start()
    {
        PhotonNetwork.Instantiate(playerPrefab.name,Vector3.zero,Quaternion.identity);
    }
}
