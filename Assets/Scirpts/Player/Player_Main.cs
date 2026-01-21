using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Main : MonoBehaviour
{

    // crear los spawn de los obstaculos, tenemos dos y los separamos por distancias
    // se van moviendo de izqueda a derecha
    // que tengan un collider entre medio para sumar puntos por distancia

    [Header("Sistema de inputs")]
    [SerializeField] InputActionAsset _playerInputSystem;

    [Header("Logic Componentes")]
    [SerializeField]Player_Controller _playerController;
    [SerializeField]Player_HitDetection _player_HitDetection;

    public event Action<PlayerState> OnPlayerState;

    public void ChangePlayerState(PlayerState state)
    {
        OnPlayerState?.Invoke(state);

        switch (state)
        {
            case PlayerState.Playing:
                HandleInputPlayerActive();
                break;
            case PlayerState.Pause:
            case PlayerState.GameOver:
                HandleInputPlayerDesactive();
                break;
        }
    }

    void HnadleStatePlayer(GameState gameState)
    {
        if(gameState == GameState.Playing || gameState == GameState.Returning)
        {
            ChangePlayerState(PlayerState.Playing);
        }
        if(gameState == GameState.Pause)
        {
            ChangePlayerState(PlayerState.Pause);
        }
        if(gameState == GameState.GameOver)
        {
            ChangePlayerState(PlayerState.GameOver);
        }
    } 

    public void HandleInputPlayerActive()
    {
        _playerInputSystem.FindActionMap("Player").Enable();
    }

    public void HandleInputPlayerDesactive()
    {
        _playerInputSystem.FindActionMap("Player").Disable();
    }


    void HandlePlayerSubscribe()
    {

        Main_Manager.Instance.Game_Manager.onStateGameCharged += HnadleStatePlayer;

        // suscripcion a señal reactiva
        //Main_Manager.Instance.Game_Manager.ReactiveGameState.Subscribe(HnadleStatePlayer);

        if (_playerController != null)
        {
            OnPlayerState += _playerController.SetMovement;
        }
        if(_player_HitDetection != null)
        {
            _player_HitDetection.OnPlayerHitDetection += _playerController.PlayerAcion;
        }

        //HnadleStatePlayer(Main_Manager.Instance.Game_Manager.CurrentState);
    }

    void HandlePlayerUnsubscribe()
    {
        Main_Manager.Instance.Game_Manager.onStateGameCharged -= HnadleStatePlayer;
        // desuscripcion a señal reactiva
        //Main_Manager.Instance.Game_Manager.ReactiveGameState.Unsubscribe(HnadleStatePlayer);

        if (_playerController != null)
        {
            OnPlayerState -= _playerController.SetMovement;
        }
        if (_player_HitDetection != null)
        {
            _player_HitDetection.OnPlayerHitDetection -= _playerController.PlayerAcion;
        }
    }

    void SuscriptionPlayerController()
    {
        if(_playerController != null)
        {
            _playerController.SubscribeInput();
        }
    }

    void UnSuscriptionPlayerController()
    {
        if(_playerController != null)
        {
            _playerController.UnsubscribeInput();
        }
    }

    private void OnEnable()
    {
        HandlePlayerSubscribe();
        HandleInputPlayerActive();
        // Suscripciones del controlador
        SuscriptionPlayerController();
    }

    private void OnDisable()
    {
        HandlePlayerUnsubscribe();
        HandleInputPlayerDesactive();
        // Desuscripciones del controlador
        UnSuscriptionPlayerController();
    }

    private void Awake()
    {
        _playerController.Initialize(_playerInputSystem);
    }

    private void Update()
    {

    }

    private void FixedUpdate()
    {
        _playerController.Jump();
    }
}
