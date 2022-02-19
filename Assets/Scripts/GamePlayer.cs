using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GamePlayer : MonoBehaviourPunCallbacks
{
    private GameObject uiManager;
    private GameObject othelloManager;
    private byte phase = 0; //0:マッチング中,1:対戦相手紹介フェーズ,2:爆弾設置フェーズ,3:ゲームフェーズ,4:リザルトフェーズ
    private byte bombCount = 3; //置ける爆弾の個数
    private List<byte> bomb = new List<byte>(); //爆弾を置いた場所を保持
    private int currentTime; //爆弾設置時間のための変数

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
                //人数が揃ったら次フェーズに
                if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
                {
                    phase = 1;
                    uiManager.GetComponent<UIGameManager>().Phase0to1();
                }
            }

            if (phase == 1)
            {
                //ここに相手の情報を受け取る内容を入れる


                uiManager.GetComponent<UIGameManager>().ChangeName();
                uiManager.GetComponent<UIGameManager>().ChangeRate();
                //ここに対戦相手紹介のTweenを入れる


                //Twewnが終わったら次フェーズに
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

                //爆弾設置フェーズに入って10秒経ったら、爆弾送受信のち次フェーズに
                if (unchecked(PhotonNetwork.ServerTimestamp - currentTime) / 1000f > 10)
                {
                    //ここにゲームスタートのTweenを入れる


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
                    //爆弾設置
                    if (phase == 2)
                    {
                        byte pos = hit.collider.gameObject.GetComponent<Othello>().GetId();

                        byte x, y, result;
                        x = (byte)(pos % 8);
                        y = (byte)(pos / 8);

                        if (!(pos == 27 || pos == 28 || pos == 35 || pos == 36))
                        {
                            //result：0→何も起きなかった、1→爆弾設置、2→爆弾削除
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
                    //オセロ設置
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
