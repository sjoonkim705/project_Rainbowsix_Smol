using Cinemachine;
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
    GameOver

}
public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static GameManager Instance { get; private set; }
    public GameState State;
    public TimelineAsset OpeningScene;
    public TimelineAsset EndingScene;
    public GameObject UI_Gameover;
    public GameObject UI_StageClear;
    public CinemachineVirtualCamera BombAreaCamera;

    [HideInInspector]
    public PlayableDirector MyPlayableManager;
    public bool IsEnding = false;

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
        AudioManager.instance.PlayBgm(true);

    }

    private void Update()
    {
        switch (State)
        {
            case GameState.Playing:
                OnPlaying();
                break;

            case GameState.CutScene:
                OnCutScene();
                break;
            default: break;

        }
    }
    private void OnPlaying()
    {
        Time.timeScale = 1f;
    }
    public void GameOver()
    {
        Time.timeScale = 0f;
        State = GameState.GameOver;
        UI_Gameover.SetActive(true);
    }
    public void OnEnding()
    {
        Time.timeScale = 0;
        State = GameState.GameOver;
        StartCoroutine(Ending_Coroutine());
    }
    private IEnumerator Ending_Coroutine()
    {
        BombAreaCamera.Priority = 11;
        yield return new WaitForSecondsRealtime(1f);
        yield return new WaitForSecondsRealtime(3f);
        UI_StageClear.SetActive(true);
    }

    private void OnCutScene()
    {
        Time.timeScale = 0;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            MyPlayableManager.time = 10f;
        }
        if (MyPlayableManager.state != PlayState.Playing && !IsEnding)
        {
            State = GameState.Playing;
        }
    }
}
