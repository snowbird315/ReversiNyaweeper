using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject panelPhase0;

    [SerializeField]
    private GameObject panelPhase2;

    [SerializeField]
    private GameObject panelPhase2_3;

    [SerializeField]
    private GameObject panelPhase3;

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
}
