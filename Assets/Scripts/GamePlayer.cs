using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GamePlayer : MonoBehaviourPunCallbacks
{
    private int x;
    private int y;
    private int turn = 0;//0:マスタークライアントのターン,1:その他のターン

    // ゲームサーバーへの接続が成功した時に呼ばれるコールバック
    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.NickName = "Master";
        }
        else
        {
            PhotonNetwork.NickName = "Slave";
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (PhotonNetwork.NickName == "Master" && turn == 0)
            {
                photonView.RPC(nameof(PutStone), RpcTarget.AllViaServer, x, y);
            }
            else if(PhotonNetwork.NickName == "Slave" && turn == 1)
            {
                photonView.RPC(nameof(PutStone), RpcTarget.AllViaServer, x, y);
            }
        }
    }

    [PunRPC]
    private void PutStone(PhotonMessageInfo info)
    {
        var position = new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f));
        PhotonNetwork.Instantiate("Square", position, Quaternion.identity);

        //送信者がマスタークライアント→その他のターン
        if (info.Sender.NickName == "Master") turn = 1;
        else turn = 0;
    }
}
