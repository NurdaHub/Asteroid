using System;
using UnityEngine;

public class MiddleAsteroid : AsteroidBase
{
    public Action<Transform> OnMiddleAsteroidBroke;

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("big collision enter");
        
        if (collision.gameObject.CompareTag("Bullet"))
        {
            OnMiddleAsteroidBroke?.Invoke(transform);
            gameObject.SetActive(false);
        }
        
        base.OnCollisionEnter2D(collision);
    }
}
