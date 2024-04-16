using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public enum GameState
{
    Playing,
    GameOver,
    LevelClear,

}
public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static GameManager Instance { get; private set; }
    public TimelineAsset OpeningScene;
    public PlayableDirector MyPlayableManager;


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
        MyPlayableManager = GetComponent<PlayableDirector>();
    }
    private void Start()
    {
        MyPlayableManager.Play(OpeningScene);
    }

    private void Update()
    {
        
    }

}
