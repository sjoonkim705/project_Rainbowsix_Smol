using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Health : MonoBehaviour
{
    public Slider HealthSlider;
    private Image _fillImage;

    // Update is called once per frame
    private void Start()
    {
        HealthSlider = GetComponentInChildren<Slider>();
        if (HealthSlider != null )
        {
            _fillImage = HealthSlider.fillRect.GetComponent<Image>();
        }
    }
    void Update()
    {
        RefreshUI();
    }
    private void RefreshUI()
    {
        HealthSlider.value = (float)Player.instance.stat.Health / Player.instance.stat.MaxHealth;
        if (HealthSlider.value < 0.2 && _fillImage != null)
        {
            _fillImage.color = Color.red;
        }
        else if (HealthSlider.value < 0.5)
        {
            _fillImage.color = Color.yellow;
        }
    }
}
