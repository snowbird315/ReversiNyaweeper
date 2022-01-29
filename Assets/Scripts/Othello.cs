using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Othello : MonoBehaviour
{
    public byte id; //pos/8 = 行数,pos%8 = 列数
    public byte status = 0; //0:なし,1:白マス,2:黒マス,3:爆弾マス
    public byte number = 0; //1~8:周囲の爆弾個数
    public bool isPut = false; //false:置けない,true:置ける
    public bool isBomb = false; //false:普通のマス,true:爆弾があるマス

    private const byte NONE = 0;
    private const byte BLACK = 1;
    private const byte WHITE = 2;
    private const byte BOMB = 3;

    private Image image;
    private GameObject child;
    private TextMeshProUGUI textMeshPro;


    // Start is called before the first frame update
    void Start()
    {
        image = this.GetComponent<Image>();
        child = transform.GetChild(0).gameObject;
        textMeshPro = child.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (status == NONE) image.color = Color.green;
        else if (status == WHITE) image.color = Color.white;
        else if (status == BLACK) image.color = Color.black;
        else if (status == BOMB) image.color = Color.red;

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
}
