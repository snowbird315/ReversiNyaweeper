using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Othello : MonoBehaviour
{
    [SerializeField]
    private Sprite img_black; //黒マス画像

    [SerializeField]
    private Sprite img_white; //白マス画像

    [SerializeField]
    private Sprite img_bomb; //爆弾画像

    [SerializeField]
    private Sprite img_put; //置ける場所画像

    public byte id; //pos/8 = 行数,pos%8 = 列数
    public byte status = 0; //0:なし,1:黒マス,2:白マス,3:爆弾マス
    public byte number = 0; //1~8:周囲の爆弾個数
    public bool isPut = false; //false:置けない,true:置ける
    public bool isBomb = false; //false:普通のマス,true:爆弾があるマス

    private const byte NONE = 0;
    private const byte BLACK = 1;
    private const byte WHITE = 2;
    private const byte BOMB = 3;

    private SpriteRenderer spriteRenderer; //オセロマスのSprite変更用
    private GameObject child; //爆弾個数の数字オブジェクトを取得する用
    private TextMeshPro textMeshPro; //爆弾個数の数字変更用


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
        //isPutに合わせてSpriteを変更
        if (isPut) spriteRenderer.sprite = img_put;

        //statusに合わせてSpriteを変更
        else if (status == NONE) spriteRenderer.sprite = null;
        else if (status == WHITE) spriteRenderer.sprite = img_white;
        else if (status == BLACK) spriteRenderer.sprite = img_black;
        else if (status == BOMB) spriteRenderer.sprite = img_bomb;

        //numberに合わせてtextを変更
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

    //押された時置けるならidを置けないなら100を返す
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
