using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneManager : MonoBehaviour
{
    //タイトルシーンに遷移
    public void GoToTitle()
    {
        SceneManager.LoadScene("TitleScene");
    }

    //ゲームシーンに遷移
    public void GoToGame()
    {
        SceneManager.LoadScene("GameScene");
    }
}