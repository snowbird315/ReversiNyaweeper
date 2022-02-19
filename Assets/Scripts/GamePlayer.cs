using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GamePlayer : MonoBehaviourPunCallbacks
{
    private GameObject uiManager;
    private GameObject othelloManager;
    private byte phase = 0; //0:�}�b�`���O��,1:�ΐ푊��Љ�t�F�[�Y,2:���e�ݒu�t�F�[�Y,3:�Q�[���t�F�[�Y,4:���U���g�t�F�[�Y
    private byte bombCount = 3; //�u���锚�e�̌�
    private List<byte> bomb = new List<byte>(); //���e��u�����ꏊ��ێ�
    private int currentTime; //���e�ݒu���Ԃ̂��߂̕ϐ�

    private void Start()
    {
        if (photonView.IsMine)
        {
            uiManager = GameObject.Find("UIGameManager");

            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.NickName = "Master";
                PhotonNetwork.Instantiate("OthelloManager", new Vector3(0, 0, 0), Quaternion.identity, 0);
            }
            else
            {
                PhotonNetwork.NickName = "Local";
                PhotonNetwork.CurrentRoom.IsOpen = false;
            }
        }
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            if (phase == 0)
            {
                //�l�����������玟�t�F�[�Y��
                if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
                {
                    phase = 1;
                    uiManager.GetComponent<UIGameManager>().Phase0to1();
                }
            }

            if (phase == 1)
            {
                //�����ɑ���̏����󂯎����e������


                uiManager.GetComponent<UIGameManager>().ChangeName();
                uiManager.GetComponent<UIGameManager>().ChangeRate();
                //�����ɑΐ푊��Љ��Tween������


                //Twewn���I������玟�t�F�[�Y��
                uiManager.GetComponent<UIGameManager>().Phase1to2();
                othelloManager = GameObject.Find("OthelloManager(Clone)");
                uiManager.GetComponent<UIGameManager>().ChangeCountText(othelloManager.GetComponent<OthelloManager>().CountOthelloMass());
                currentTime = PhotonNetwork.ServerTimestamp;
                phase = 2;
            }

            if (phase == 2)
            {
                float time = (float)(10 + (currentTime - PhotonNetwork.ServerTimestamp) / 1000f);
                uiManager.GetComponent<UIGameManager>().ChangeTimeSlider(time);
                uiManager.GetComponent<UIGameManager>().ChangeTimeText(time);

                //���e�ݒu�t�F�[�Y�ɓ�����10�b�o������A���e����M�̂����t�F�[�Y��
                if (unchecked(PhotonNetwork.ServerTimestamp - currentTime) / 1000f > 10)
                {
                    //�����ɃQ�[���X�^�[�g��Tween������


                    phase = 3;
                    uiManager.GetComponent<UIGameManager>().Phase2to3();
                    for (byte i = 0; i < bombCount; i++)
                    {
                        byte id;
                        Random.InitState(System.DateTime.Now.Millisecond);
                        do
                        {
                            id = (byte)Random.Range(0, 64);
                        } while (id == 27 || id == 28 || id == 35 || id == 36);
                        bomb.Add(id);
                    }

                    othelloManager.GetComponent<OthelloManager>().initOthelloMath();
                    for (byte i = 0; i < 3; i++)
                    {
                        othelloManager.GetComponent<OthelloManager>().Bomb(bomb[i]);
                    }
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = new Ray();
                RaycastHit2D hit = new RaycastHit2D();
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                hit = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction);

                if (hit.collider.CompareTag("Othello"))
                {
                    //���e�ݒu
                    if (phase == 2)
                    {
                        byte pos = hit.collider.gameObject.GetComponent<Othello>().GetId();

                        byte x, y, result;
                        x = (byte)(pos % 8);
                        y = (byte)(pos / 8);

                        if (!(pos == 27 || pos == 28 || pos == 35 || pos == 36))
                        {
                            //result�F0�������N���Ȃ������A1�����e�ݒu�A2�����e�폜
                            result = othelloManager.GetComponent<OthelloManager>().PutBomb(x, y, bombCount);
                            if (result == 1)
                            {
                                bomb.Add(pos);
                                bombCount--;
                            }
                            else if (result == 2)
                            {
                                bomb.Remove(pos);
                                bombCount++;
                            }
                        }
                    }
                    //�I�Z���ݒu
                    else if (phase == 3)
                    {
                        byte pos = hit.collider.gameObject.GetComponent<Othello>().OnUserPush();

                        if (pos != 100)
                        {
                            byte x, y;
                            byte[] count = new byte[2];
                            x = (byte)(pos % 8);
                            y = (byte)(pos / 8);

                            if(othelloManager.GetComponent<OthelloManager>().PushOthello(PhotonNetwork.NickName, x, y))
                            {
                                count = othelloManager.GetComponent<OthelloManager>().CountOthelloMass();
                                photonView.RPC(nameof(ChangeTextCount), RpcTarget.All, count);
                            }
                        }
                    }
                }
            }
        }
    }

    [PunRPC]
    private void ChangeTextCount(byte[] count)
    {
        uiManager.GetComponent<UIGameManager>().ChangeCountText(count);
    }
}
