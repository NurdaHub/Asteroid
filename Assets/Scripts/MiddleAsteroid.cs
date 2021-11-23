using System;
using UnityEngine;

public class MiddleAsteroid : AsteroidBase
{
    public Action<Transform> OnMiddleAsteroidBroke;
    private int points = 50;

    protected override void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Bullet"))
        {
            UIController.currentScore += points;
            OnMiddleAsteroidBroke?.Invoke(transform);
            gameObject.SetActive(false);
        }
        
        base.OnTriggerEnter2D(collider);
    }
}
