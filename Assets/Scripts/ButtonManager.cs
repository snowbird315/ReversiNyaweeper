using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void PushResult()
    {
        SceneManager.LoadScene("TitleScene");
    }

    public void PushRetry()
    {
        SceneManager.LoadScene("GameScene");
    }
}