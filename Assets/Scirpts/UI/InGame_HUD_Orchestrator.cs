using UnityEngine;
using TMPro;

public class InGame_HUD_Orchestrator : MonoBehaviour
{
    [Header("Views / Panels")]
    [SerializeField] GameObject _hudPlayingGroup; // El que tiene el score
    [SerializeField] GameObject _pauseGroup;      // El menú de pausa
    [SerializeField] GameObject _gameOverGroup;   // El menú de muerte

    [SerializeField] TMP_Text _hudPlayingGroupText; // El texto del score

    private void OnEnable()
    {
        // Suscripción a las señales del Game_Manager
        Main_Manager.Instance.Game_Manager.onStateGameCharged += SwitchView;
        Main_Manager.Instance.Game_Manager.onScoreChanged += UpdateScore;
    }

    private void OnDisable()
    {
        Main_Manager.Instance.Game_Manager.onStateGameCharged -= SwitchView;
        Main_Manager.Instance.Game_Manager.onScoreChanged -= UpdateScore;
    }

    void SwitchView(GameState state)
    {
        // Limpiamos todo primero para evitar solapamientos (el "miedo" a que algo salga mal)
        _hudPlayingGroup.SetActive(false);
        _pauseGroup.SetActive(false);
        _gameOverGroup.SetActive(false);

        // Activamos solo lo que corresponde
        switch (state)
        {
            case GameState.Playing:
            case GameState.Returning:
                _hudPlayingGroup.SetActive(true);
                break;

            case GameState.Pause:
                _pauseGroup.SetActive(true);
                break;

            case GameState.GameOver:
                _gameOverGroup.SetActive(true);
                break;

            case GameState.Waiting:
                // Si estamos en el menú principal, el HUD inyectado debe estar oculto
                break;
        }
    }

    void UpdateScore(int score)
    {
        _hudPlayingGroupText.text = score.ToString();
    }

    // --- Métodos para los Botones (Inyectar en el Inspector) ---
    public void OnClickResume() => Main_Manager.Instance.Game_Manager.ChangeState(GameState.Returning);
    public void OnClickRestart() => Main_Manager.Instance.Game_Manager.ResetGame();
    public void OnClickExitToMenu() => Main_Manager.Instance.Game_Manager.RetunrMenu();
}
