using System;
using UnityEngine;

public class Game_Manager : MonoBehaviour
{
    public event Action onStartGame;
    public event Action onResetGame;
    public event Action onGameOver;

    public event Action<GameState> onStateGameCharged;
    public event Action<int> onScoreChanged;

    public int _score { get; private set; }
    int _scoreByLevel;

    public GameState CurrentState { get; private set; }

    // Test señal reactiva 
    // Descartado hasta pulir
    // public readonly Evento_Reactivo<GameState> ReactiveGameState = new Evento_Reactivo<GameState>();

    // --------------------

    public void ChangeState(GameState state)
    {
        CurrentState = state;

        onStateGameCharged.Invoke(state);

        // test reactiv signal
        // ReactiveGameState.Value = state;
        // --------------------

        switch (CurrentState)
        {
            case GameState.Playing:
                Time.timeScale = 1;
                break;
            case GameState.Pause:
                Time.timeScale = 0;
                break;
            case GameState.Returning:
                Time.timeScale = 1;
                break;
            case GameState.GameOver:
                Time.timeScale = 0;
                onGameOver?.Invoke(); 
                // dispara el cambio de ventana en el hud inyectable
                break;

        }
    }

    public void StartGame()
    {
        onStartGame?.Invoke();
        _score = 0;
        ChangeState(GameState.Playing);
        onScoreChanged?.Invoke(_score);
        Main_Manager.Instance.Scene_Manager.LoadScene("Level_1");
    }

    public void ResetGame()
    {
        if (CurrentState == GameState.GameOver)
        {
            onResetGame?.Invoke();
            _score = _scoreByLevel;
            onScoreChanged?.Invoke(_scoreByLevel);
            ChangeState(GameState.Playing);
        }

    }

    public void RetunrMenu()
    {
        Main_Manager.Instance.Scene_Manager.LoadScene("Menu_Main");
        ChangeState(GameState.Waiting);
    }

    public void QuitGame()
    {
        Application.Quit();
        /*
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
        */
    }

    public void AddScore(int score)
    {
        if(CurrentState == GameState.Playing)
        {
            _score += score;
            onScoreChanged?.Invoke(_score);
            NextLevel();
        }
    }

    void NextLevel()
    {
        if(_score >= 100)
        {
            ChangeState(GameState.Pause);
            Main_Manager.Instance.Scene_Manager.LoadScene("Level_2");
            _scoreByLevel = _score;
            onScoreChanged?.Invoke(_scoreByLevel);
            ChangeState(GameState.Playing);
        }
    }

    public void ResetSignalInGame(bool restSiganls)
    {
        if(restSiganls)
        {
            GameState actualState = CurrentState;
            onStateGameCharged?.Invoke(actualState);
        }
    }
}
