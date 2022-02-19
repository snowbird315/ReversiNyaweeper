using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class OthelloManager : MonoBehaviourPunCallbacks
{
    private byte turn = 1; //1:�}�X�^�[�N���C�A���g�̃^�[��,2:���[�J���N���C�A���g�̃^�[��

    private const byte NONE = 0;
    private const byte BLACK = 1;
    private const byte WHITE = 2;
    private const byte BOMB = 3;

    private Othello[,] othelloBlocks = new Othello[8, 8]; //�I�Z���}�X��ێ�

    // Start is called before the first frame update
    void Start()
    {
        CreateOthlelloMass();
        init();
    }
 
    //�I�Z���}�X����
    private void CreateOthlelloMass()
    {
        for (byte i = 0; i < 8; i++)
        {
            for (byte j = 0; j < 8; j++)
            {
                Vector3 position = new Vector3((float)(-3.5 + i), (float)(1.0 - j), 0);
                GameObject obj = (GameObject)Instantiate(Resources.Load("Othello"), position, Quaternion.identity);
                obj.name = "Othello" + (i + j * 8).ToString();
                othelloBlocks[i, j] = obj.GetComponent<Othello>();
                othelloBlocks[i, j].id = (byte)(i + j * 8);
            }
        }
    }

    //�I�Z���}�X�J�E���g
    public byte[] CountOthelloMass()
    {
        byte[] result = new byte[] { 0, 0 };

        for (byte i = 0; i < 8; i++)
        {
            for (byte j = 0; j < 8; j++)
            {
                if (othelloBlocks[i, j].status == BLACK) result[0]++;
                else if (othelloBlocks[i, j].status == WHITE) result[1]++;
            }
        }

        return result;
    }

    //�I�Z���}�X���͂̔��e���J�E���g
    private void CountBomb()
    {
        for(byte i = 0; i < 8; i++)
        {
            for(byte j = 0; j < 8; j++)
            {
                othelloBlocks[i, j].number = 0;
            }
        }

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
                            if (!CheckPosition(i + k, j + l)) continue;
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
        AllCheckPut();
    }

    //�S�Ă̏ꏊ�ɂ����ă{���̕ێ����폜(�X�e�[�^�X�͕ύX�Ȃ�)
    public void initOthelloMath()
    {
        for (byte i = 0; i < 8; i++)
        {
            for (byte j = 0; j < 8; j++)
            {
                othelloBlocks[i, j].status = NONE;
            }
        }
        init();
    }

    //�S�Ă̏ꏊ�ɂ����Ēu���邩�ǂ����I�Z���}�X�X�V
    private bool AllCheckPut()
    {
        bool result = false;
        for (byte i = 0; i < 8; i++)
        {
            for (byte j = 0; j < 8; j++)
            {
                othelloBlocks[i, j].isPut = CheckPut(i, j);
                if (othelloBlocks[i, j].isPut) result = true;
            }
        }
        return result;
    }

    //x,y�ɂ����Ēu���邩�ǂ�������
    private bool CheckPut(byte x, byte y)
    {
        bool result = false;
        Vector2Int pos = new Vector2Int(0, 0);

        //�󂫃}�X�łȂ����false
        if (othelloBlocks[x, y].status != 0) return result;

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
                if (othelloBlocks[pos.x, pos.y].status != (1 - (turn - 1)) + 1) continue;

                //��ȏ��΂�����
                while (true)
                {
                    pos.x += i;
                    pos.y += j;
                    if (!CheckPosition(pos.x, pos.y)) break;
                    if (othelloBlocks[pos.x, pos.y].status == NONE) break;
                    if (othelloBlocks[pos.x, pos.y].status == turn) result = true;
                }
            }
        }
        return result;
    }

    //�Ђ�����Ԃ�
    private void ReversePut(byte x, byte y)
    {
        Vector2Int pos = new Vector2Int(0, 0);

        ChangeStone(x, y);

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
                if (othelloBlocks[pos.x, pos.y].status != (1 - (turn - 1)) + 1) continue;

                //��ȏ��΂�����
                while (true)
                {
                    pos.x += i;
                    pos.y += j;
                    if (!CheckPosition(pos.x, pos.y)) break;
                    if (othelloBlocks[pos.x, pos.y].status == NONE) break;
                    if (othelloBlocks[pos.x, pos.y].status == turn)
                    {
                        Vector2Int reversePos = new Vector2Int(x, y);
                        while (true)
                        {
                            reversePos.x += i;
                            reversePos.y += j;
                            if (reversePos == pos) break;
                            ChangeStone(reversePos.x, reversePos.y);
                        }
                    }
                }
            }
        }
    }

    //�I�Z���Ղ͈͓̔��ɂ��邩�`�F�b�N
    private bool CheckPosition(int x, int y)
    {
        if (x < 0 || 7 < x || y < 0 || 7 < y) return false;
        else return true;
    }

    //��1,��2�����̏ꏊ��status���3�����ɕύX
    private void ChangeStone(int x, int y)
    {
        othelloBlocks[x, y].status = turn;
    }

    //�I������
    private void EndGame()
    {
        
    }
    

    //���e�ݒu�t�F�[�Y���FPlayer����I�Z���}�X�������ꂽ��
    public byte PutBomb(byte x, byte y, byte count)
    {
        byte result = 0; //0:�ω��Ȃ�,1:���e�ǉ�,2:���e�폜

        if (othelloBlocks[x, y].status == BOMB)
        {
            othelloBlocks[x, y].status = NONE;
            result = 2;
        }
        else if(count > 0)
        {
            othelloBlocks[x, y].status = BOMB;
            result = 1;
        }
        return result;
    }

    //�Q�[���T�[�o�[���ɂ���l�ɑ΂���id�̎����I�Z���}�X�ɔ��e��ݒu
    public void Bomb(byte id)
    {
        photonView.RPC(nameof(ShereBomb), RpcTarget.All, id);
    }

    //�Q�[���t�F�[�Y���FPlayer����I�Z���}�X�������ꂽ���̔���
    public bool PushOthello(string name, byte x, byte y)
    {
        bool result = false;
        if ((name == "Master" && turn == BLACK) || (name == "Local" && turn == WHITE))
        {
            photonView.RPC(nameof(PutStone), RpcTarget.All, x, y);
            result = true;
        }
        return result;
    }

    //�Q�[���T�[�o�[���ɂ���l�ɑ΂��Ĕ��e�ݒu���s
    [PunRPC]
    private void ShereBomb(byte id)
    {
        byte x, y;
        x = (byte)(id % 8);
        y = (byte)(id / 8);
        othelloBlocks[x, y].isBomb = true;
        CountBomb();
    }

    //�Q�[���T�[�o�[���ɂ���l�ɑ΂��ăI�Z���}�X�ݒu���s
    [PunRPC]
    private void PutStone(byte x, byte y, PhotonMessageInfo info)
    {
        if (othelloBlocks[x, y].isBomb) othelloBlocks[x, y].status = BOMB;
        else
        {
            ReversePut(x, y);
        }

        //���M�҂��}�X�^�[�N���C�A���g�����[�J���N���C�A���g�̃^�[��
        if (info.Sender.NickName == "Master") turn = 2;
        else turn = 1;

        if (!AllCheckPut())
        {
            if (info.Sender.NickName == "Master") turn = 1;
            else turn = 2;

            if (!AllCheckPut()) EndGame();
        }
    }
}