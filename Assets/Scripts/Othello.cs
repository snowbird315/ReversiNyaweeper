using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Othello : MonoBehaviour
{
    public byte id; //pos/8 = �s��,pos%8 = ��
    public byte status = 0; //0:�Ȃ�,1:���}�X,2:���}�X,3:���e�}�X
    public byte number = 0; //1~8:���͂̔��e��
    public bool isPut = false; //false:�u���Ȃ�,true:�u����
    public bool isBomb = false; //false:���ʂ̃}�X,true:���e������}�X
    public bool bomb = false;

    private const byte NONE = 0;
    private const byte BLACK = 1;
    private const byte WHITE = 2;
    private const byte BOMB = 3;

    private SpriteRenderer sprite;
    private GameObject child;
    private TextMeshPro textMeshPro;


    // Start is called before the first frame update
    void Start()
    {
        sprite = this.GetComponent<SpriteRenderer>();
        child = transform.GetChild(0).gameObject;
        textMeshPro = child.GetComponent<TextMeshPro>();
    }

    // Update is called once per frame
    void Update()
    {
        if (status == NONE) sprite.color = Color.green;
        else if (status == WHITE) sprite.color = Color.white;
        else if (status == BLACK) sprite.color = Color.black;
        else if (status == BOMB) sprite.color = Color.red;

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

        if (isPut) sprite.color = Color.yellow;

        if (bomb) sprite.color = Color.red;
    }

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
