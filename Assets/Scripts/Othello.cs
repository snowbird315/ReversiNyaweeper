using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Othello : MonoBehaviour
{
    [SerializeField]
    private Sprite img_black; //���}�X�摜

    [SerializeField]
    private Sprite img_white; //���}�X�摜

    [SerializeField]
    private Sprite img_bomb; //���e�摜

    [SerializeField]
    private Sprite img_put; //�u����ꏊ�摜

    public byte id; //pos/8 = �s��,pos%8 = ��
    public byte status = 0; //0:�Ȃ�,1:���}�X,2:���}�X,3:���e�}�X
    public byte number = 0; //1~8:���͂̔��e��
    public bool isPut = false; //false:�u���Ȃ�,true:�u����
    public bool isBomb = false; //false:���ʂ̃}�X,true:���e������}�X

    private const byte NONE = 0;
    private const byte BLACK = 1;
    private const byte WHITE = 2;
    private const byte BOMB = 3;

    private SpriteRenderer spriteRenderer; //�I�Z���}�X��Sprite�ύX�p
    private GameObject child; //���e���̐����I�u�W�F�N�g���擾����p
    private TextMeshPro textMeshPro; //���e���̐����ύX�p


    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        child = transform.GetChild(0).gameObject;
        textMeshPro = child.GetComponent<TextMeshPro>();
    }

    // Update is called once per frame
    void Update()
    {
        //isPut�ɍ��킹��Sprite��ύX
        if (isPut) spriteRenderer.sprite = img_put;

        //status�ɍ��킹��Sprite��ύX
        else if (status == NONE) spriteRenderer.sprite = null;
        else if (status == WHITE) spriteRenderer.sprite = img_white;
        else if (status == BLACK) spriteRenderer.sprite = img_black;
        else if (status == BOMB) spriteRenderer.sprite = img_bomb;

        //number�ɍ��킹��text��ύX
        if (number != 0) textMeshPro.text = number.ToString();
        else textMeshPro.text = "";
        if (number == 1) textMeshPro.color = new Color(0, 0.38f, 1f, 1f);
        if (number == 2) textMeshPro.color = new Color(0.29f, 0.82f, 0.18f, 1f);
        if (number == 3) textMeshPro.color = Color.red;
        if (number == 4) textMeshPro.color = new Color(0, 0, 0.7f, 1f);
        if (number == 5) textMeshPro.color = new Color(0.5f, 0.4f, 0.26f, 1f);
        if (number == 6) textMeshPro.color = new Color(0, 0.7f, 1f, 1f);
        if (number == 7) textMeshPro.color = Color.black;
        if (number == 8) textMeshPro.color = new Color(0.5f, 0.5f, 0.5f, 1f);
    }

    //�����ꂽ���u����Ȃ�id��u���Ȃ��Ȃ�100��Ԃ�
    public byte OnUserPush()
    {
        if (isPut)
        {
            return id;
        }
        else
        {
            return (byte)100;
        }
    }

    public byte GetId()
    {
        return id;
    }
}
