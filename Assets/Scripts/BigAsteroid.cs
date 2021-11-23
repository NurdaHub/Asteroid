using System;
using UnityEngine;

public class BigAsteroid : AsteroidBase
{
    public Action<Transform> OnBigAsteroidBroke;
    private int points = 20;

    protected override void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Bullet"))
        {
            UIController.currentScore += points;
            OnBigAsteroidBroke?.Invoke(transform);
            gameObject.SetActive(false);
        }
        
        base.OnTriggerEnter2D(collider);
    }
}
