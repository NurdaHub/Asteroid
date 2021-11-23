using System;
using UnityEngine;

public class SmallAsteroid : AsteroidBase
{
    public Action OnSmallAsteroidBroke;
    private int points = 100;

    private void OnBroke()
    {
        gameObject.SetActive(false);
        OnSmallAsteroidBroke?.Invoke();
    }
    
    protected override void OnTriggerEnter2D(Collider2D collider)
    {
        var isDestroyer = collider.CompareTag("Alien") || collider.CompareTag("Player");
        
        if (isDestroyer)
            OnBroke();

        if (collider.CompareTag("Bullet"))
        {
            UIController.currentScore += points;
            OnBroke();
        }
    }
}
