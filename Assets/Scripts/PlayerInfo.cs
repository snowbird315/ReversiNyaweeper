using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    public static string name = "Test";
    public static int rate = 9999;
    public static int money = 10000;
    public static int win = 0;
    public static int lose = 0;

    public static string GetName()
    {
        return name;
    }

    public static int GetRate()
    {
        return rate;
    }

    public static int GetMoney()
    {
        return money;
    }

    public static int GetWin()
    {
        return win;
    }

    public static int GetLose()
    {
        return lose;
    }
}
