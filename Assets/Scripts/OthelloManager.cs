using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class OthelloManager : MonoBehaviourPunCallbacks
{
    public byte x;
    public byte y;
    private byte pos; //�I�Z���}�X�̍��W�������l
    private byte turn = 0;//0:�}�X�^�[�N���C�A���g�̃^�[��,1:���[�J���N���C�A���g�̃^�[��

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
        AllCheckPut();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = new Ray();
            RaycastHit2D hit = new RaycastHit2D();
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            hit = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction);

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
    
    //�I�Z���}�X����
    private void CreateOthlloMass()
    {
        for (byte i = 0; i < 8; i++)
        {
            for (byte j = 0; j < 8; j++)
            {
                Vector3 position = new Vector3((float)(-3.5 + i), (float)(-3.5 + j), 0);
                GameObject obj = (GameObject)Instantiate(Resources.Load("Othello"), position, Quaternion.identity);
                othelloBlocks[i, j] = obj.GetComponent<Othello>();
                othelloBlocks[i, j].id = (byte)(i + j * 8);
            }
        }
    }

    //�I�Z���}�X���͂̔��e���J�E���g
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

    //������
    private void init()
    {
        othelloBlocks[3, 3].status = WHITE;
        othelloBlocks[4, 4].status = WHITE;
        othelloBlocks[3, 4].status = BLACK;
        othelloBlocks[4, 3].status = BLACK;
    }

    //�S�Ă̏ꏊ�ɂ����Ēu���邩�ǂ����I�Z���}�X�X�V
    private void AllCheckPut()
    {
        for (byte i = 0; i < 8; i++)
        {
            for (byte j = 0; j < 8; j++)
            {
                CheckPut(i, j, false);
            }
        }
    }

    //x,y�ɂ����Ēu���邩�ǂ����I�Z���}�X�X�V�A��3�����Ftrue���Ђ�����Ԃ��Afalse�����肾��
    private void CheckPut(byte x, byte y, bool put)
    {
        bool result = false;
        Vector2Int pos = new Vector2Int(0, 0);

        //�󂫃}�X�łȂ����false
        if (othelloBlocks[x, y].status != 0) othelloBlocks[x, y].isPut = result;

        //����������
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                //�ה���
                if (i == 0 && j == 0) continue;
                pos.x = x + i;
                pos.y = y + j;
                if (!CheckPosition(pos.x, pos.y)) continue;
                if (othelloBlocks[pos.x, pos.y].status != (byte)(1 - turn)) continue;

                //��ȏ��΂�����
                while (true)
                {
                    pos.x += i;
                    pos.y += j;
                    if (!CheckPosition(pos.x, pos.y)) break;
                    if (othelloBlocks[pos.x, pos.y].status == NONE) break;
                    if (othelloBlocks[pos.x, pos.y].status == turn)
                    {
                        if (put)
                        {
                            if (othelloBlocks[x, y].isBomb)
                            {
                                ChangeStone(x, y, BOMB);
                            }
                            else
                            {
                                Vector2Int reversePos = new Vector2Int(x, y);
                                ChangeStone(x, y, turn);
                                while (true)
                                {
                                    reversePos.x += i;
                                    reversePos.y += j;
                                    if (reversePos == pos) break;
                                    ChangeStone(reversePos.x, reversePos.y, turn);
                                }
                            }
                        }
                        else result = true;
                    }
                }
            }
        }
        othelloBlocks[x, y].isPut = result;
    }
    
    //�I�Z���Ղ͈͓̔��ɂ��邩�`�F�b�N
    private bool CheckPosition(int x, int y)
    {
        if (x < 0 || 7 < x || y < 0 || 7 < y) return false;
        else return true;
    }

    //��1,��2�����̏ꏊ��status���3�����ɕύX
    private void ChangeStone(int x, int y, byte turn)
    {
        othelloBlocks[x, y].status = (byte)(turn + 1);
    }

    //�Q�[���T�[�o�[���ɂ���l�ɑ΂��Ď��s
    [PunRPC]
    private void PutStone(byte x, byte y, PhotonMessageInfo info)
    {
        if (othelloBlocks[x, y].isBomb) othelloBlocks[x, y].status = BOMB;
        else
        {
            CheckPut(x, y, true);
            AllCheckPut();
        }

        //���M�҂��}�X�^�[�N���C�A���g�����[�J���N���C�A���g�̃^�[��
        if (info.Sender.NickName == "Master") turn = 1;
        else turn = 0;
    }
}