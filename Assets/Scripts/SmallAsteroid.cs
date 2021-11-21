using System;
using UnityEngine;

public class SmallAsteroid : AsteroidBase
{
    public Action OnSmallAsteroidBroke;
    
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        var collisionGO = collision.gameObject;
        var isDestroyer = collisionGO.CompareTag("Alien") || collisionGO.CompareTag("Player") || collisionGO.CompareTag("Bullet");
        
        if (isDestroyer)
        {
            gameObject.SetActive(false);
            OnSmallAsteroidBroke?.Invoke();
            Debug.Log("broke   " + transform.position);
        }
    }
}
