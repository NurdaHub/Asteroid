using UnityEngine;

public class BulletController : BulletBase
{
    private string asteroidTag = "Asteroid";
    private string alienTag = "Alien";
    private void OnTriggerEnter2D(Collider2D collider)
    {
        var isDestroyer = collider.CompareTag(asteroidTag) || collider.CompareTag(alienTag);
        
        if (isDestroyer)
            this.gameObject.SetActive(false);
    }
}
