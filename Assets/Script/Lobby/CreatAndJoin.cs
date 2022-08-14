using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using Photon.Pun;
public class CreatAndJoin : MonoBehaviourPunCallbacks
{
    [SerializeField]private byte maxplayer = 10; // 設定每房人數最大上限
    public InputField CreateInput;
    public InputField JoinInput;
    //public Text RoomList;
    private int Maxroom = 0;
    ArrayList roomName = new ArrayList();
    public void Update()
    {
        /*
        if(roomName.Count > Maxroom)
        {
            RoomList.text = "";
            for (int i = 0; i < roomName.Count; i++)
            {
                RoomList.text += roomName[i] + "\n";
                
            }
            Maxroom = roomName.Count;
        }
        */
    }
    public void CreateRoom()
    {
       PhotonNetwork.CreateRoom(CreateInput.text,new Photon.Realtime.RoomOptions{ MaxPlayers = maxplayer});
       roomName.Add((string)CreateInput.text);
    }
    
    public void JoinRoom()
    {
       PhotonNetwork.JoinRoom(JoinInput.text);
    }

    public override void OnJoinedRoom()
    {
       PhotonNetwork.LoadLevel("GameRoom");
    }
}
