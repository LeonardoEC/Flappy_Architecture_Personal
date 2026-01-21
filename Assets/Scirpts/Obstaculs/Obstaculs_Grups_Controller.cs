using System.Collections;
using UnityEngine;

public class Obstaculs_Grups_Controller : MonoBehaviour
{
    [SerializeField]float _obstaculsSpeed = 5f;
    [SerializeField]Rigidbody2D _obstaculeRigidybody;
    //Coroutine _obstaculeLivingTime;

    //-17
    /*
    float _timeToDesactive = 4f;
    float _remainingTime;
    */

    bool _canMove;

    void SuscriptionBySignals()
    {
        Main_Manager.Instance.Game_Manager.onStateGameCharged += HandleStateGame;
        //Main_Manager.Instance.Game_Manager.onStateGameCharged += HandleCorrutin;
    }

    void UnSuscriptionBySignals()
    {
        Main_Manager.Instance.Game_Manager.onStateGameCharged -= HandleStateGame;
        //Main_Manager.Instance.Game_Manager.onStateGameCharged -= HandleCorrutin;
    }

    private void OnEnable()
    {
        SuscriptionBySignals();
        ObstaculeMovenet();
    }

    private void OnDisable()
    {
        UnSuscriptionBySignals();
    }

    private void Awake()
    {

    }

    void HandleStateGame(GameState state)
    {
        if (state == GameState.Playing || state == GameState.Returning)
        {
            _canMove = true;
            DesactivObstaculeByDistance();
        }
        if (state == GameState.Pause || state == GameState.GameOver)
        {
            _canMove = false;
        }

        Debug.Log(gameObject.activeSelf);
    }



    void ObstaculeMovenet()
    {
        if(gameObject.activeSelf || _canMove)
        {
            _obstaculeRigidybody.linearVelocity = Vector2.left * _obstaculsSpeed;
        }
        else
        {
            _obstaculeRigidybody.linearVelocity = Vector2.zero;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Despawner"))
        {
            gameObject.SetActive(false);
        }

        if (collision.CompareTag("Player"))
        {
            Main_Manager.Instance.Game_Manager.AddScore(1);
        }
    }

    /*
    void HandleCorrutin(GameState state)
    {
        if (_obstaculeLivingTime != null)
        {
            StopCoroutine(_obstaculeLivingTime);
        }
        if (gameObject.activeSelf)
        {
            _remainingTime = _timeToDesactive;
            _obstaculeLivingTime = StartCoroutine(DesactiveObstacule());
        }
        if(state == GameState.Returning)
        {
            _obstaculeLivingTime = StartCoroutine(DesactiveObstacule());
        }
        if (state == GameState.Pause)
        {
            StopCoroutine(_obstaculeLivingTime);
        }
        if(state == GameState.GameOver)
        {
            StopCoroutine(_obstaculeLivingTime);
        }
    }
    */


    void DesactivObstaculeByDistance()
    {
        if(transform.position.x <= -17f)
        {
            gameObject.SetActive(false);
        }
    }

    /*
    IEnumerator DesactiveObstacule()
    {
        yield return new WaitForSeconds(_remainingTime);
        gameObject.SetActive(false);
        _obstaculeLivingTime = null;
    }
    */
}
