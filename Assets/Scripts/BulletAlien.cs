using UnityEngine;

public class BulletAlien : BulletBase
{
    private void OnTriggerEnter2D(Collider2D collider)
    {
        var isDestroyer = collider.CompareTag("Asteroid") || collider.CompareTag("Player");
        
        if (isDestroyer)
            this.gameObject.SetActive(false);
    }
}
