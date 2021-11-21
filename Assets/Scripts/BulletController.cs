using UnityEngine;

public class BulletController : BulletBase
{
    private void OnTriggerEnter2D(Collider2D collider)
    {
        var isDestroyer = collider.CompareTag("Asteroid") || collider.CompareTag("Alien");
        
        if (isDestroyer)
        {
            UIController.currentScore++;
            this.gameObject.SetActive(false);
        }
    }
}
