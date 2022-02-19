using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

// MonoBehaviourPunCallbacks���p�����āAPUN�̃R�[���o�b�N���󂯎���悤�ɂ���
public class PhotonManager : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        // PhotonServerSettings�̐ݒ���e���g���ă}�X�^�[�T�[�o�[�֐ڑ�����
        PhotonNetwork.ConnectUsingSettings();
    }

    // �}�X�^�[�T�[�o�[�ւ̐ڑ��������������ɌĂ΂��R�[���o�b�N
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    //�Q�[���T�[�o�[�ւ̐ڑ������s�������ɌĂ΂��R�[���o�b�N
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        //�V�������[�����쐬�A�ő�l����2�l��
        var roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;

        PhotonNetwork.CreateRoom(null, roomOptions);
    }

    // �Q�[���T�[�o�[�ւ̐ڑ��������������ɌĂ΂��R�[���o�b�N
    public override void OnJoinedRoom()
    {
        PhotonNetwork.Instantiate("GamePlayer", new Vector3(0, 0, 0), Quaternion.identity, 0);
    }

    //���[���̍쐬�������������ɌĂ΂��R�[���o�b�N
    public override void OnCreatedRoom()
    {
        
    }

    // ���v���C���[�����[���֎Q���������ɌĂ΂��R�[���o�b�N
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        
    }

    // ���v���C���[�����[������ޏo�������ɌĂ΂��R�[���o�b�N
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        //PhotonNetwork.LeaveRoom();
    }
}
