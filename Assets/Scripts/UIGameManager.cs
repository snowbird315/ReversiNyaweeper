using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject panelPhase0;
    [SerializeField]
    private GameObject panelPhase2;
    [SerializeField]
    private GameObject panelPhase2_3;
    [SerializeField]
    private GameObject panelPhase3;

    [SerializeField]
    private GameObject textTime;
    [SerializeField]
    private GameObject sliderTime;

    [SerializeField]
    private GameObject imageMasterIcon;
    [SerializeField]
    private GameObject textMasterOthelloCount;
    [SerializeField]
    private GameObject textMasterName;
    [SerializeField]
    private GameObject textMasterRate;

    [SerializeField]
    private GameObject imageLocalIcon;
    [SerializeField]
    private GameObject textLocalOthelloCount;
    [SerializeField]
    private GameObject textLocalName;
    [SerializeField]
    private GameObject textLocalRate;

    private void Start()
    {
        panelPhase0.SetActive(true);
        panelPhase2.SetActive(false);
        panelPhase2_3.SetActive(false);
        panelPhase3.SetActive(false);
    }

    public void Phase0to1()
    {
        panelPhase0.SetActive(false);
    }

    public void Phase1to2()
    {
        panelPhase2.SetActive(true);
        panelPhase2_3.SetActive(true);
    }

    public void Phase2to3()
    {
        panelPhase2.SetActive(false);
        panelPhase3.SetActive(true);
    }

    public void ChangeTimeSlider(float time)
    {
        sliderTime.GetComponent<Slider>().value = float.Parse(time.ToString("F1")) / 10f;
    }

    public void ChangeTimeText(float time)
    {
        textTime.GetComponent<Text>().text = time.ToString("F1");
    }

    public void ChangeCountText(byte[] count)
    {
        textMasterOthelloCount.GetComponent<Text>().text = count[0].ToString();
        textLocalOthelloCount.GetComponent<Text>().text = count[1].ToString();
    }

    public void ChangeName()
    {
        textMasterName.GetComponent<Text>().text = "Master";
        textLocalName.GetComponent<Text>().text = "Local";
    }

    public void ChangeRate()
    {
        textMasterRate.GetComponent<Text>().text = "9999";
        textLocalRate.GetComponent<Text>().text = "9999";
    }
}
