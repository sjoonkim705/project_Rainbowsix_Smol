using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_DisarmBomb : MonoBehaviour
{
    private Slider DisarmProcessSlider;
    public BombTrigger BombTriggerProcess;

    private void Awake()
    {
        DisarmProcessSlider = GetComponentInChildren<Slider>();

    }
    void Update()
    {
        DisarmProcessSlider.value = BombTriggerProcess.DisarmProcess;
    }
    public void RefreshUI()
    {
        DisarmProcessSlider.value = BombTriggerProcess.DisarmProcess;
    }
}
