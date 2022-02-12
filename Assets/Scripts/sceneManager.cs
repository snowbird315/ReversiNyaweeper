using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneManager : MonoBehaviour
{
    public void GoToTitle()
    {
        SceneManager.LoadScene("TitleScene");
    }

    public void GoToGame()
    {
        SceneManager.LoadScene("GameScene");
    }
}