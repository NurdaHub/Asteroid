using UnityEngine;

public class MiddleAsteroid : AsteroidBase
{
    private int points = 50;
    private string bulletTag = "Bullet";

    protected override void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag(bulletTag))
        {
            UIController.currentScore += points;
            AsteroidSpawner.OnMiddleAsteroidBroke?.Invoke(this.transform);
            gameObject.SetActive(false);
        }
        
        base.OnTriggerEnter2D(collider);
    }
}
