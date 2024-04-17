using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public enum GameState
{
    Playing,
    CutScene,
    LevelClear,

}
public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static GameManager Instance { get; private set; }
    public GameState State;
    public TimelineAsset OpeningScene;
    [HideInInspector]
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
        if (MyPlayableManager != null)
        {
            State = GameState.CutScene;
        }
        

    }

    private void Update()
    {
        if (MyPlayableManager.state == PlayState.Playing)
        {
            State = GameState.CutScene;
        }
        else
        {
            State = GameState.Playing;
        }
        switch (State)
        {
            case GameState.Playing:
                OnPlaying();
                break;
            case GameState.CutScene:
                OnCutScene();
                break;
        }
    }
    private void OnPlaying()
    {

    }
    private void OnCutScene()
    {
        Debug.Log("CutScene");
    }
}
