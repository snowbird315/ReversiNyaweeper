using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneManager : MonoBehaviour
{
    //�^�C�g���V�[���ɑJ��
    public void GoToTitle()
    {
        SceneManager.LoadScene("TitleScene");
    }

    //�Q�[���V�[���ɑJ��
    public void GoToGame()
    {
        SceneManager.LoadScene("GameScene");
    }
}