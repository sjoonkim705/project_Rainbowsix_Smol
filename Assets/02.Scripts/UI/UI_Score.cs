using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Score : MonoBehaviour
{
    public static UI_Score Instance { get; private set; }
    public Text ScoreNumber;
    private int _score = 0;
    public int Score
    {
        get { return _score; }
        set 
        { 
            _score = value;
            RefreshUI();
        }
    }


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        ScoreNumber.text = $"{Score:D3}";
    }
    public void RefreshUI()
    {
        ScoreNumber.text = $"{Score:D3}";
    }
}
