using UnityEngine;

public class MiddleAsteroid : AsteroidBase
{
    private int points = 50;

    protected override void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Bullet"))
        {
            UIController.currentScore += points;
            AsteroidSpawner.OnMiddleAsteroidBroke?.Invoke(this.transform);
            gameObject.SetActive(false);
        }
        
        base.OnTriggerEnter2D(collider);
    }
}
