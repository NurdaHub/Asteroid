using System;
using UnityEngine;

public class BigAsteroid : AsteroidBase
{
    public Action<Transform> OnBigAsteroidBroke;

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Debug.Log("big collision enter   " + transform.position);
            OnBigAsteroidBroke?.Invoke(transform);
            gameObject.SetActive(false);
        }
        
        base.OnCollisionEnter2D(collision);
    }
}
