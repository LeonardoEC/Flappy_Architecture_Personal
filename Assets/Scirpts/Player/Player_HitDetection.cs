using System;
using UnityEngine;

public class Player_HitDetection : MonoBehaviour
{
    public event Action<string> OnPlayerHitDetection;
    // public event Action<int> OnValueCoin;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Coin"))
        {
            OnPlayerHitDetection?.Invoke(collision.gameObject.tag);
        }
        if(collision.CompareTag("Obstacul"))
        {
            OnPlayerHitDetection?.Invoke(collision.gameObject.tag);
        }
    }


}
