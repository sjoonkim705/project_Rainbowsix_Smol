using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_EnemyHealthBar : MonoBehaviour
{
    private Slider HealthSliderUI;
    private Enemy MyEnemy;

    private void Awake()
    {
        HealthSliderUI = GetComponentInChildren<Slider>();
        MyEnemy = GetComponentInParent<Enemy>();
    }
    private void Update()
    {
        RefreshUI();
    }
    private void RefreshUI()
    {
        HealthSliderUI.value = (float)MyEnemy.stat.Health / MyEnemy.stat.MaxHealth;

        if (HealthSliderUI.value == 1f || HealthSliderUI.value == 0f)
        {
            HealthSliderUI.enabled = false;
        }
        else
        {
            HealthSliderUI.enabled = true;
        }
    }

}
