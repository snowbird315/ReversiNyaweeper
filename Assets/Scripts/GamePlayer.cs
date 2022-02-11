using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GamePlayer : MonoBehaviourPunCallbacks
{
    private GameObject uiManager;
    private GameObject othelloManager;
    private byte phase = 0; //0:マッチング中,1:対戦相手紹介フェーズ,2:爆弾設置フェーズ,3:ゲームフェーズ,4:リザルトフェーズ
    private byte bombCount = 5;
    private List<byte> bomb = new List<byte>();
    private int currentTime;

    private void Start()
    {
        if (photonView.IsMine)
        {
            uiManager = GameObject.Find("UIManager");

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
                if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
                {
                    phase = 1;
                    uiManager.GetComponent<UIManager>().Phase0to1();
                }
            }

            if (phase == 1)
            {
                uiManager.GetComponent<UIManager>().Phase1to2();
                othelloManager = GameObject.Find("OthelloManager(Clone)");
                currentTime = PhotonNetwork.ServerTimestamp;
                phase = 2;
            }

            if (phase == 2)
            {
                if (unchecked(PhotonNetwork.ServerTimestamp - currentTime) / 1000f > 10)
                {
                    phase = 3;
                    uiManager.GetComponent<UIManager>().Phase2to3();
                    for (byte i = 0; i < (5 - bomb.Count); i++)
                    {

                    }
                    photonView.RPC(nameof(SendBomb), RpcTarget.All, bomb);
                    othelloManager.GetComponent<OthelloManager>().initBomb();


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
                    if (phase == 2)
                    {
                        byte pos = hit.collider.gameObject.GetComponent<Othello>().GetId();

                        byte x, y, result;
                        x = (byte)(pos % 8);
                        y = (byte)(pos / 8);

                        if (!(pos == 27 || pos == 28 || pos == 35 || pos == 36))
                        {
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
                    else if (phase == 3)
                    {
                        byte pos = hit.collider.gameObject.GetComponent<Othello>().OnUserPush();

                        if (pos != 100)
                        {
                            byte x, y;
                            x = (byte)(pos % 8);
                            y = (byte)(pos / 8);

                            othelloManager.GetComponent<OthelloManager>().PushOthello(PhotonNetwork.NickName, x, y);
                        }
                    }
                }
            }
        }
    }

    [PunRPC]
    private void SendBomb(List<byte> bomb)
    {
        othelloManager.GetComponent<OthelloManager>().Bomb(bomb);
    }

    public string GetNickName()
    {
        return PhotonNetwork.NickName;
    }
}
