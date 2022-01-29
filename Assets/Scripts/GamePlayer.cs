using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GamePlayer : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.NickName = "Master";
        }
        else
        {
            PhotonNetwork.NickName = "Local";
        }

        if(PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
        }
    }

    private void Update()
    {
        
    }
}
