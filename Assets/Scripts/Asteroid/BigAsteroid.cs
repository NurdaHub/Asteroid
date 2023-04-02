using UI;
using UnityEngine;

public class BigAsteroid : AsteroidBase
{
    private int points = 20;
    private string bulletTag = "Bullet";

    protected override void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag(bulletTag))
        {
            UIController.currentScore += points;
            AsteroidSpawner.OnBigAsteroidBroke?.Invoke(this.transform);
            gameObject.SetActive(false);
        }
        
        base.OnTriggerEnter2D(collider);
    }
}
