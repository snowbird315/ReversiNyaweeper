using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class OthelloManager : MonoBehaviourPunCallbacks
{
    public byte x;
    public byte y;
    private byte pos; //オセロマスの座標を示す値
    private byte turn = 0;//0:マスタークライアントのターン,1:ローカルクライアントのターン

    public GameObject parentOthllo;

    private const byte NONE = 0;
    private const byte BLACK = 1;
    private const byte WHITE = 2;
    private const byte BOMB = 3;

    private Othello[,] othelloBlocks = new Othello[8,8];

    // Start is called before the first frame update
    void Start()
    {
        parentOthllo = GameObject.Find("CanvasGame");
        CreateOthlloMass();
        CountBomb();
        init();
        CheckPut(false);
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = new Ray();
            RaycastHit hit = new RaycastHit();
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity))
            {
                Debug.Log("a");
                if (hit.collider.CompareTag("Othello"))
                {
                    pos = hit.collider.gameObject.GetComponent<Othello>().OnUserPush();
                    if (pos != 100)
                    {
                        x = (byte)(pos / 8);
                        y = (byte)(pos % 8);
                        if (PhotonNetwork.NickName == "Master" && turn == 0)
                        {
                            photonView.RPC(nameof(PutStone), RpcTarget.AllViaServer, x, y);
                        }
                        else if (PhotonNetwork.NickName == "Local" && turn == 1)
                        {
                            photonView.RPC(nameof(PutStone), RpcTarget.AllViaServer, x, y);
                        }
                    }
                }
            }
        }
    }

    private void CreateOthlloMass()
    {
        for (byte i = 0; i < 8; i++)
        {
            for (byte j = 0; j < 8; j++)
            {
                Vector3 position = new Vector3(-480 + 60 + (120 * i), 120 + (-120 * j), 0);
                GameObject obj = (GameObject)Instantiate(Resources.Load("Othello"), parentOthllo.transform);
                obj.transform.localPosition = position;
                othelloBlocks[i, j] = obj.GetComponent<Othello>();
                othelloBlocks[i, j].id = (byte)(i + j * 8);
            }
        }
    }

    private void CountBomb()
    {
        for(byte i = 0; i < 8; i++)
        {
            for(byte j = 0; j < 8; j++)
            {
                if (othelloBlocks[i, j].isBomb)
                {
                    for(int k = -1; k <= 1; k++)
                    {
                        for(int l = -1; l <= 1; l++)
                        {
                            if (k == 0 && l == 0) continue;
                            othelloBlocks[i + k, j + l].number += 1;
                        }
                    }
                }
            }
        }
    }

    private void init()
    {
        othelloBlocks[3, 3].status = WHITE;
        othelloBlocks[4, 4].status = WHITE;
        othelloBlocks[3, 4].status = BLACK;
        othelloBlocks[4, 3].status = BLACK;
    }

    private void CheckPut(bool put)
    {
        Vector2 pos = new Vector2(0, 0);

        for (byte i = 0; i < 8; i++)
        {
            for (byte j = 0; j < 8; j++)
            {
                bool result = false;

                //空きマスでなければfalse
                if (othelloBlocks[i, j].status != 0) continue;

                //八方向判定
                for (int k = -1; k <= 1; k++)
                {
                    for (int l = -1; l <= 1; l++)
                    {
                        //隣判定
                        if (k == 0 && l == 0) continue;
                        pos.x = i + k;
                        pos.y = j + l;
                        if(!CheckPosition((int)pos.x,(int)pos.y)) continue;
                        if (othelloBlocks[(byte)pos.x, (byte)pos.y].status != (byte)(1 - turn)) continue;

                        //一つ以上飛ばし判定
                        while (true)
                        {
                            pos.x += k;
                            pos.y += l;
                            if (!CheckPosition((int)pos.x, (int)pos.y)) break;
                            if (othelloBlocks[(byte)pos.x, (byte)pos.y].status == NONE) break;
                            if (othelloBlocks[(byte)pos.x,(byte)pos.y].status == turn)
                            {
                                if (put)
                                {
                                    Vector2 reversePos = new Vector2(i, j);
                                    while (true)
                                    {
                                        reversePos.x += k;
                                        reversePos.y += l;
                                        if (reversePos == pos) break;
                                        ChangeStone((byte)reversePos.x, (byte)reversePos.y, turn);
                                    }
                                }
                                else result = true;
                            }
                        }
                    }
                }

                othelloBlocks[i, j].isPut = result;
            }
        }
    }
    
    private bool CheckPosition(int x, int y)
    {
        if (x < 0 || 7 < x || y < 0 || 7 < y) return false;
        else return true;
    }

    private void ChangeStone(byte x, byte y, byte turn)
    {
        othelloBlocks[x, y].status = (byte)(turn + 1);
    }

    [PunRPC]
    private void PutStone(byte x, byte y, PhotonMessageInfo info)
    {
        if (othelloBlocks[x, y].isBomb) othelloBlocks[x, y].status = BOMB;
        else
        {
            ChangeStone(x, y, turn);
            CheckPut(true);
            CheckPut(false);
        }

        //送信者がマスタークライアント→ローカルクライアントのターン
        if (info.Sender.NickName == "Master") turn = 1;
        else turn = 0;
    }
}