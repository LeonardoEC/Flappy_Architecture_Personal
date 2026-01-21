using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Controller : MonoBehaviour
{

    [Header("Engine Componentes")]
    [SerializeField] Rigidbody2D _playerRigidbody;
    [SerializeField] Player_Animation _playerAnimation;

    [Header("Player Value")]
    [SerializeField] float _jumpForce;

    InputAction jumpAction;
    bool _canMove;
    bool _isJumped;


    public void SetMovement(PlayerState state)
    {
        if(state == PlayerState.Playing)
        {
            _canMove = true;
        }
        if( state == PlayerState.Pause || state == PlayerState.GameOver)
        {
            _canMove = false;
        }
    }

    public void Initialize(InputActionAsset inputs)
    {
        jumpAction = inputs.FindAction("Jump");
    }

    public void SubscribeInput()
    {
        jumpAction.performed += ApplyJumpForce;
    }

    public void UnsubscribeInput()
    {
        jumpAction.performed -= ApplyJumpForce;
    }

    public void ApplyJumpForce(InputAction.CallbackContext context)
    {
        _isJumped = true;
    }

    public void Jump()
    {
        if (_isJumped && _canMove)
        {
            _playerRigidbody.linearVelocity = Vector2.up * _jumpForce;
        }
        _isJumped = false;

    }

    public void PlayerAcion(string tag)
    {
        if(tag == "Coin")
        {
            AddPoint();
        }
        if(tag == "Obstacul")
        {
            GameOver();
        }
    }

    void AddPoint()
    {
        return;
    }


    void GameOver()
    {
        Main_Manager.Instance.Game_Manager.ChangeState(GameState.GameOver);
    }

    void Pause()
    {
        Main_Manager.Instance.Game_Manager.ChangeState(GameState.Pause);
    }
}
