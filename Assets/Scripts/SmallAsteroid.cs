using UnityEngine;

public class SmallAsteroid : AsteroidBase
{
    private int points = 100;
    private string bulletTag = "Bullet";

    private void OnBroke()
    {
        gameObject.SetActive(false);
        AsteroidSpawner.OnAsteroidBroke?.Invoke();
    }
    
    protected override void OnTriggerEnter2D(Collider2D collider)
    {
        var isDestroyer = collider.CompareTag(alienTag) || collider.CompareTag(playerTag);
        
        if (isDestroyer)
            OnBroke();

        if (collider.CompareTag(bulletTag))
        {
            UIController.currentScore += points;
            OnBroke();
        }
    }
}
