using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Health : MonoBehaviour
{
    public Slider HealthSlider;

    // Update is called once per frame
    private void Start()
    {
        HealthSlider = GetComponentInChildren<Slider>();
    }
    void Update()
    {
        RefreshUI();
    }
    private void RefreshUI()
    {
        HealthSlider.value = (float)Player.instance.stat.Health / Player.instance.stat.MaxHealth;
    }
}
