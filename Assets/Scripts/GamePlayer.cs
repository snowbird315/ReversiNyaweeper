using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GamePlayer : MonoBehaviourPunCallbacks
{
    private int x;
    private int y;
    private int turn = 0;//0:�}�X�^�[�N���C�A���g�̃^�[��,1:���̑��̃^�[��

    // �Q�[���T�[�o�[�ւ̐ڑ��������������ɌĂ΂��R�[���o�b�N
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

        //���M�҂��}�X�^�[�N���C�A���g�����̑��̃^�[��
        if (info.Sender.NickName == "Master") turn = 1;
        else turn = 0;
    }
}
